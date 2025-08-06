namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ResultBindMapTests
{
    [TestMethod]
    public void Bind_WithValidValue_ReturnsSuccess()
    {
        // arrange
        var input = Result<string>.Success("42");

        // act
        var result = input.Bind(v => ResultHelper.TryParse(v));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsFailure()
    {
        // arrange
        var input = Result<string>.Success("non-int-text");

        // act
        var result = input.Bind(v => ResultHelper.TryParse(v));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void Bind_WithExistingFailure_ReturnsFailure()
    {
        // arrange
        var input = Result<string>.Failure(Error.NotFound("id", "error"));

        // act
        var result = input.Bind([ExcludeFromCodeCoverage] (v) => ResultHelper.TryParse(v));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void Map_WithValidValue_ReturnsSuccess()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = input.Map(v => v * 2);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(84);
    }

    [TestMethod]
    public void Map_WithFailure_ReturnsFailure()
    {
        // arrange
        var input = Result<int>.Failure(Error.Invalid("code", "test"));

        // act
        var result = input.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void MapErrors_WithFailure_ReturnsNewFailure()
    {
        // arrange
        var input = Result<int>.Failure(Error.Invalid("code", "test"));

        // act
        var result = input.MapErrors<string>();

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void MapErrors_WithSuccess_Throws()
    {
        // arrange
        var input = Result<int>.Success(1);

        // act
        Assert.Throws<InvalidOperationException>([ExcludeFromCodeCoverage]() => input.MapErrors<string>());
    }
}

namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ResultToTests
{
    [TestMethod]
    public void ToArray_WithSuccessValue_ReturnsSingleItemArray()
    {
        // arrange
        var input = Result<string>.Success("test");

        // act
        var result = input.ToArray();

        // assert
        result.Should().HaveCount(1);
        result.Should().Contain("test");
    }

    [TestMethod]
    public void ToArray_WithFailure_ReturnsEmptyArray()
    {
        // arrange
        var input = Result<string>.Failure(Error.Invalid("code", "test"));

        // act
        var result = input.ToArray();

        // assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToList_WithSuccessValue_ReturnsSingleItemList()
    {
        // arrange
        var input = Result<string>.Success("test");

        // act
        var result = input.ToList();

        // assert
        result.Should().HaveCount(1);
        result.Should().Contain("test");
    }

    [TestMethod]
    public void ToList_WithFailure_ReturnsEmptyList()
    {
        // arrange
        var input = Result<string>.Failure(Error.Invalid("code", "test"));

        // act
        var result = input.ToList();

        // assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToOptional_WithSuccessValue_ReturnsSome()
    {
        // arrange
        var input = Result<string>.Success("test");

        // act
        var result = input.ToOptional();

        // assert
        result.IsSome.Should().BeTrue();
    }

    [TestMethod]
    public void ToOptional_WithFailure_ReturnsNone()
    {
        // arrange
        var input = Result<string>.Failure(Error.Invalid("code", "test"));

        // act
        var result = input.ToOptional();

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void ToString_WithSuccessValue_ReturnsSuccessString()
    {
        // arrange
        var input = Result<int>.Success(13);

        // act
        var result = input.ToString();

        // assert
        result.Should().Be("Success<Int32>(value = 13)");
    }

    [TestMethod]
    public void ToString_WithFailure_ReturnsFailureString()
    {
        // arrange
        var input = Result<int>.Failure(Error.Invalid("code", "test"));

        // act
        var result = input.ToString();

        // assert
        result.Should().Be("Failure<Int32>(errors = Error (code [7]): test)");
    }
}

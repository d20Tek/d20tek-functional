namespace D20Tek.Functional.UnitTests;

[TestClass]
public class TryExceptTests
{
    [TestMethod]
    public void Run_WithSuccessfulOperation_ReturnsSuccess()
    {
        // arrange

        // act
        var result = TryExcept.Run(
            () => Result<int>.Success(42),
            [ExcludeFromCodeCoverage] (ex) => Result<int>.Failure(ex));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void Run_WithOperationException_ReturnsFailure()
    {
        // arrange

        // act
        var result = TryExcept.Run(
            () => throw new InvalidOperationException(),
            ex => Result<int>.Failure(ex));

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public void Bind_WithValidValue_ReturnsSuccess()
    {
        // arrange
        var input = "42";

        // act
        var result = TryExcept.Bind(
            () => input,
            x => Result<int>.Success(int.Parse(x)));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsSuccess()
    {
        // arrange
        var input = "non-int-text";

        // act
        var result = TryExcept.Bind(
            () => input,
            [ExcludeFromCodeCoverage] (x) => Result<int>.Success(int.Parse(x)));

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public void Map_WithValidValue_ReturnsSuccess()
    {
        // arrange
        var input = 42;

        // act
        var result = TryExcept.Map(() => input, v => v * 2);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(84);
    }

    [TestMethod]
    public void Map_WithExceptionThrown_ReturnsFailure()
    {
        // arrange
        var input = 42;

        // act
        var result = TryExcept.Map(() => input, v => v / 0);

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }
}

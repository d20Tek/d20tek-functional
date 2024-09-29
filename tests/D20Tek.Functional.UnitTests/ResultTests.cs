namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ResultTests
{
    [TestMethod]
    public void IsSuccess_WithValue_ReturnsTrue()
    {
        // arrange
        var result = Result<int>.Success(42);

        // act
        var isSuccess = result.IsSuccess;
        var isFailure = result.IsFailure;

        // assert
        isSuccess.Should().BeTrue();
        isFailure.Should().BeFalse();
    }

    [TestMethod]
    public void IsFailure_WithError_ReturnsTrue()
    {
        // arrange
        var result = Result<string>.Failure(Error.Unexpected("Test.Error", "Error message."));

        // act
        var isSuccess = result.IsSuccess;
        var isFailure = result.IsFailure;

        // assert
        isSuccess.Should().BeFalse();
        isFailure.Should().BeTrue();
    }

    [TestMethod]
    public void IsFailure_WithErrors_ReturnsTrue()
    {
        // arrange
        var result = Result<string>.Failure(
        [
            Error.Unexpected("Test.Error", "Error message."),
            Error.Validation("Invalid", "Validation error."),
            Error.Validation("foo", "bar")
        ]);

        // act
        var isSuccess = result.IsSuccess;
        var isFailure = result.IsFailure;

        // assert
        isSuccess.Should().BeFalse();
        isFailure.Should().BeTrue();
    }

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
}

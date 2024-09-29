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
}

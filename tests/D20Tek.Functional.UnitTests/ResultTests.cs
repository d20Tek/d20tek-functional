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

    [TestMethod]
    public void Count_WithSuccessValue_ReturnsOne()
    {
        // arrange
        var result = Result<DateTimeOffset>.Success(DateTimeOffset.Now);

        // act
        int count = result.Count();

        // assert
        count.Should().Be(1);
    }

    [TestMethod]
    public void Count_WithFailureError_ReturnsZero()
    {
        // arrange
        var result = Result<DateTimeOffset>.Failure(Error.Unexpected("code", "test"));

        // act
        int count = result.Count();

        // assert
        count.Should().Be(0);
    }

    [TestMethod]
    public void DefaultValue_WithSuccessValue_ReturnsValue()
    {
        // arrange
        var input = Result<int>.Success(10);

        // act
        var result = input.DefaultValue(42);

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public void DefaultValue_WithFailure_ReturnsDefault()
    {
        // arrange
        var input = Result<int>.Failure(Error.Conflict("code", "test"));

        // act
        var result = input.DefaultValue(42);

        // assert
        result.Should().Be(42);
    }

    [TestMethod]
    public void DefaultWith_WithSuccessValue_ReturnsValue()
    {
        // arrange
        var x = 10;
        var input = Result<int>.Success(x);

        // act
        var result = input.DefaultWith([ExcludeFromCodeCoverage] () => x + 1);

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public void DefaultWith_WithFailure_ReturnsDefault()
    {
        // arrange
        var defDate = DateTimeOffset.Now;
        var input = Result<DateTimeOffset>.Failure(Error.Failure("code", "test"));

        // act
        var result = input.DefaultWith(() => defDate);

        // assert
        result.Should().Be(defDate);
    }
}

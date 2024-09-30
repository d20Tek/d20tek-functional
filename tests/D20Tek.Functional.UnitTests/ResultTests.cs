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
    public void IsFailure_WithException_ReturnsTrue()
    {
        // arrange
        var result = Result<string>.Failure(new InvalidOperationException());

        // act
        var isSuccess = result.IsSuccess;
        var isFailure = result.IsFailure;

        // assert
        isSuccess.Should().BeFalse();
        isFailure.Should().BeTrue();
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

    [TestMethod]
    public void Exists_WithSuccessAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = input.Exists(x => x > 10);

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Exists_WithSuccessAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = input.Exists(x => x == 10);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Exists_WithNone_ReturnsFalse()
    {
        // arrange
        var input = Result<string>.Failure(Error.Create("code", "test", 404));

        // act
        var result = input.Exists([ExcludeFromCodeCoverage] (x) => x == string.Empty);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Filter_WithSuccessAndPositivePredicate_ReturnsSuccess()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = input.Filter(x => x >= 5);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public void Filter_WithSuccessAndNegativePredicate_ReturnsNotFoundError()
    {
        // arrange
        var input = Result<int>.Success(4);

        // act
        var result = input.Filter(x => x >= 5);
        var errors = result.GetErrors();

        // assert
        result.IsFailure.Should().BeTrue();
        errors.Length.Should().Be(1);
        errors.First().Code.Should().Be("Filter.Error");
        errors.First().Message.Should().Contain("found");
        errors.First().Type.Should().Be(ErrorType.NotFound);
    }

    [TestMethod]
    public void Filter_WithFailure_ReturnsFailure()
    {
        // arrange
        var input = Result<int>.Failure(Error.Invalid("code", "error"));

        // act
        var result = input.Filter([ExcludeFromCodeCoverage] (x) => x >= 5);

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public void Fold_WithSuccessValue_ReturnsAccumulated()
    {
        // arrange
        var input = Result<int>.Success(1);

        // act
        var result = input.Fold(0, (acc, value) => acc + value * 2);

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public void Fold_WithSuccessValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var input = Result<int>.Success(2);

        // act
        var result = input.Fold(10, (acc, value) => acc + value * 2);

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public void Fold_WithFailure_ReturnsInitial()
    {
        // arrange
        var input = Result<int>.Failure(Error.Unauthorized("code", "test"));

        // act
        var result = input.Fold(5, [ExcludeFromCodeCoverage] (acc, value) => acc + value * 2);

        // assert
        result.Should().Be(5);
    }

    [TestMethod]
    public void FoldBack_WithSuccessValue_ReturnsAccumulated()
    {
        // arrange
        var input = Result<int>.Success(1);

        // act
        var result = input.FoldBack(0, (value, acc) => acc + value * 2);

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public void FoldBack_WithSuccessValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var input = Result<int>.Success(2);

        // act
        var result = input.FoldBack(10, (value, acc) => acc + value * 2);

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public void FoldBack_WithFailure_ReturnsInitial()
    {
        // arrange
        var input = Result<int>.Failure(Error.Forbidden("code", "test"));

        // act
        var result = input.FoldBack(5, [ExcludeFromCodeCoverage] (value, acc) => acc + value * 2);

        // assert
        result.Should().Be(5);
    }

    [TestMethod]
    public void ForAll_WithSuccessAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = input.ForAll(x => x >= 5);

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ForAll_WithSuccessAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var input = Result<int>.Success(4);

        // act
        var result = input.ForAll(x => x >= 5);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ForAll_WithFailure_ReturnsTrue()
    {
        // arrange
        var input = Result<string>.Failure(Error.Unexpected("code", "test"));

        // act
        var result = input.ForAll([ExcludeFromCodeCoverage] (x) => x == string.Empty);

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetValue_WithFailure_ThrowsException()
    {
        // arrange
        var result = Result<decimal>.Failure(Error.Invalid("code", "test"));

        // act
        _ = result.GetValue();
    }

    [TestMethod]
    public void GetErrors_WithFailure_ReturnsError()
    {
        // arrange
        var result = Result<decimal>.Failure(Error.Validation("code", "test"));

        // act
        var errors = result.GetErrors();

        // assert
        errors.Length.Should().Be(1);
        errors.First().ToString().Should().Be("Error (code [2]): test");
    }

    [TestMethod]
    public void GetErrors_WithSuccess_ReturnsEmptyErrors()
    {
        // arrange
        var result = Result<decimal>.Success(42);

        // act
        var errors = result.GetErrors();

        // assert
        errors.Should().BeEmpty();
    }

    [TestMethod]
    public void Iter_WithSuccessValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var result = Result<string>.Success("Hello World");

        // act
        result.Iter(x => output = x);

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public void Iter_WithFailure_DoesNotPerformAction()
    {
        // arrange
        var output = string.Empty;
        var result = Result<string>.Failure(Error.Invalid("code", "test"));

        // act
        result.Iter([ExcludeFromCodeCoverage] (x) => output = "foo");

        // assert
        output.Should().Be(string.Empty);
    }
}

using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class ResultAsyncTests
{
    [TestMethod]
    public async Task DefaultWithAsync_WithSuccessValue_ReturnsValue()
    {
        // arrange
        var x = 10;
        var input = Result<int>.Success(x);

        // act
        var result = await input.DefaultWithAsync([ExcludeFromCodeCoverage] () => Task.FromResult(x + 1));

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public async Task TaskDefaultWithAsync_WithSuccessValue_ReturnsValue()
    {
        // arrange
        var x = 10;
        var input = Task.FromResult(Result<int>.Success(x));

        // act
        var result = await input.DefaultWithAsync([ExcludeFromCodeCoverage] () => Task.FromResult(x + 1));

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public async Task DefaultWithAsync_WithFailure_ReturnsDefault()
    {
        // arrange
        var defDate = DateTimeOffset.Now;
        var input = Result<DateTimeOffset>.Failure(Error.Failure("code", "test"));

        // act
        var result = await input.DefaultWithAsync(() => Task.FromResult(defDate));

        // assert
        result.Should().Be(defDate);
    }

    [TestMethod]
    public async Task TaskDefaultWithAsync_WithFailure_ReturnsDefault()
    {
        // arrange
        var defDate = DateTimeOffset.Now;
        var input = Task.FromResult(Result<DateTimeOffset>.Failure(Error.Failure("code", "test")));

        // act
        var result = await input.DefaultWithAsync(() => Task.FromResult(defDate));

        // assert
        result.Should().Be(defDate);
    }

    [TestMethod]
    public async Task ExistsAsync_WithSuccessAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = await input.ExistsAsync(x => Task.FromResult(x > 10));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskExistsAsync_WithSuccessAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(42));

        // act
        var result = await input.ExistsAsync(x => Task.FromResult(x > 10));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task ExistsAsync_WithSuccessAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = await input.ExistsAsync(x => Task.FromResult(x == 10));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task TaskExistsAsync_WithSuccessAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(42));

        // act
        var result = await input.ExistsAsync(x => Task.FromResult(x == 10));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task ExistsAsync_WithNone_ReturnsFalse()
    {
        // arrange
        var input = Result<string>.Failure(Error.Create("code", "test", 404));

        // act
        var result = await input.ExistsAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x == string.Empty));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task TaskExistsAsync_WithNone_ReturnsFalse()
    {
        // arrange
        var input = Task.FromResult(Result<string>.Failure(Error.Create("code", "test", 404)));

        // act
        var result = await input.ExistsAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x == string.Empty));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task FilterAsync_WithSuccessAndPositivePredicate_ReturnsSuccess()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = await input.FilterAsync(x => Task.FromResult(x >= 5));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task TaskFilterAsync_WithSuccessAndPositivePredicate_ReturnsSuccess()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(42));

        // act
        var result = await input.FilterAsync(x => Task.FromResult(x >= 5));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task FilterAsync_WithSuccessAndNegativePredicate_ReturnsNotFoundError()
    {
        // arrange
        var input = Result<int>.Success(4);

        // act
        var result = await input.FilterAsync(x => Task.FromResult(x >= 5));
        var errors = result.GetErrors();

        // assert
        result.IsFailure.Should().BeTrue();
        errors.Length.Should().Be(1);
        errors.First().Code.Should().Be("Filter.Error");
        errors.First().Message.Should().Contain("found");
        errors.First().Type.Should().Be(ErrorType.NotFound);
    }

    [TestMethod]
    public async Task TaskFilterAsync_WithSuccessAndNegativePredicate_ReturnsNotFoundError()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(4));

        // act
        var result = await input.FilterAsync(x => Task.FromResult(x >= 5));
        var errors = result.GetErrors();

        // assert
        result.IsFailure.Should().BeTrue();
        errors.Length.Should().Be(1);
        errors.First().Code.Should().Be("Filter.Error");
        errors.First().Message.Should().Contain("found");
        errors.First().Type.Should().Be(ErrorType.NotFound);
    }

    [TestMethod]
    public async Task FilterAsync_WithFailure_ReturnsFailure()
    {
        // arrange
        var input = Result<int>.Failure(Error.Invalid("code", "error"));

        // act
        var result = await input.FilterAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x >= 5));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskFilterAsync_WithFailure_ReturnsFailure()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Failure(Error.Invalid("code", "error")));

        // act
        var result = await input.FilterAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x >= 5));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task FoldAsync_WithSuccessValue_ReturnsAccumulated()
    {
        // arrange
        var input = Result<int>.Success(1);

        // act
        var result = await input.FoldAsync(0, (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task TaskFoldAsync_WithSuccessValue_ReturnsAccumulated()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(1));

        // act
        var result = await input.FoldAsync(0, (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task FoldAsync_WithSuccessValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var input = Result<int>.Success(2);

        // act
        var result = await input.FoldAsync(10, (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public async Task TaskFoldAsync_WithSuccessValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(2));

        // act
        var result = await input.FoldAsync(10, (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public async Task FoldAsync_WithFailure_ReturnsInitial()
    {
        // arrange
        var input = Result<int>.Failure(Error.Unauthorized("code", "test"));

        // act
        var result = await input.FoldAsync(
            5,
            [ExcludeFromCodeCoverage] (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(5);
    }

    [TestMethod]
    public async Task TaskFoldAsync_WithFailure_ReturnsInitial()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Failure(Error.Unauthorized("code", "test")));

        // act
        var result = await input.FoldAsync(
            5,
            [ExcludeFromCodeCoverage] (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(5);
    }

    [TestMethod]
    public async Task FoldBackAsync_WithSuccessValue_ReturnsAccumulated()
    {
        // arrange
        var input = Result<int>.Success(1);

        // act
        var result = await input.FoldBackAsync(0, (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task TaskFoldBackAsync_WithSuccessValue_ReturnsAccumulated()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(1));

        // act
        var result = await input.FoldBackAsync(0, (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task FoldBackAsync_WithSuccessValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var input = Result<int>.Success(2);

        // act
        var result = await input.FoldBackAsync(10, (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public async Task TaskFoldBackAsync_WithSuccessValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(2));

        // act
        var result = await input.FoldBackAsync(10, (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public async Task FoldBackAsync_WithFailure_ReturnsInitial()
    {
        // arrange
        var input = Result<int>.Failure(Error.Forbidden("code", "test"));

        // act
        var result = await input.FoldBackAsync(
            5,
            [ExcludeFromCodeCoverage] (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(5);
    }

    [TestMethod]
    public async Task TaskFoldBackAsync_WithFailure_ReturnsInitial()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Failure(Error.Forbidden("code", "test")));

        // act
        var result = await input.FoldBackAsync(
            5,
            [ExcludeFromCodeCoverage] (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(5);
    }

    [TestMethod]
    public async Task ForAllAsync_WithSuccessAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = await input.ForAllAsync(x => Task.FromResult(x >= 5));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskForAllAsync_WithSuccessAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(42));

        // act
        var result = await input.ForAllAsync(x => Task.FromResult(x >= 5));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task ForAllAsync_WithSuccessAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var input = Result<int>.Success(4);

        // act
        var result = await input.ForAllAsync(x => Task.FromResult(x >= 5));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task TaskForAllAsync_WithSuccessAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Success(4));

        // act
        var result = await input.ForAllAsync(x => Task.FromResult(x >= 5));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task ForAllAsync_WithFailure_ReturnsTrue()
    {
        // arrange
        var input = Result<string>.Failure(Error.Unexpected("code", "test"));

        // act
        var result = await input.ForAllAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x == string.Empty));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskForAllAsync_WithFailure_ReturnsTrue()
    {
        // arrange
        var input = Task.FromResult(Result<string>.Failure(Error.Unexpected("code", "test")));

        // act
        var result = await input.ForAllAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x == string.Empty));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task IterAsync_WithSuccessValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var result = Result<string>.Success("Hello World");

        // act
        await result.IterAsync(x => Task.FromResult(output = x));

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public async Task TaskIterAsync_WithSuccessValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var result = Task.FromResult(Result<string>.Success("Hello World"));

        // act
        await result.IterAsync(x => Task.FromResult(output = x));

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public async Task IterAsync_WithFailure_DoesNotPerformAction()
    {
        // arrange
        var output = string.Empty;
        var result = Result<string>.Failure(Error.Invalid("code", "test"));

        // act
        await result.IterAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(output = "foo"));

        // assert
        output.Should().Be(string.Empty);
    }

    [TestMethod]
    public async Task TaskIterAsync_WithFailure_DoesNotPerformAction()
    {
        // arrange
        var output = string.Empty;
        var result = Task.FromResult(Result<string>.Failure(Error.Invalid("code", "test")));

        // act
        await result.IterAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(output = "foo"));

        // assert
        output.Should().Be(string.Empty);
    }
}

using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class TryExceptAsyncTests
{
    [TestMethod]
    public async Task RunAsync_WithSuccessfulOperation_ReturnsSuccess()
    {
        // arrange

        // act
        var result = await TryExceptAsync.RunAsync(
            () => Task.FromResult(Result<int>.Success(42)),
            [ExcludeFromCodeCoverage] (ex) => Result<int>.Failure(ex));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task RunAsync_WithOperationException_ReturnsFailure()
    {
        // arrange

        // act
        var result = await TryExceptAsync.RunAsync(
            () => throw new InvalidOperationException(),
            ex => Result<int>.Failure(ex));

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task RunAsync_WithSuccessfulOperation_RunsFinally()
    {
        // arrange
        bool ranFinally = false;

        // act
        var result = await TryExceptAsync.RunAsync(
            () => Task.FromResult(Result<int>.Success(42)),
            [ExcludeFromCodeCoverage] (ex) => Result<int>.Failure(ex),
            () => ranFinally = true);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
        ranFinally.Should().BeTrue();
    }

    [TestMethod]
    public async Task BindAsync_WithValidValue_ReturnsSuccess()
    {
        // arrange
        var input = Task.FromResult("42");

        // act
        var result = await TryExceptAsync.BindAsync(
            () => input,
            x => Task.FromResult(Result<int>.Success(int.Parse(x))));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsSuccess()
    {
        // arrange
        var input = Task.FromResult("non-int-text");

        // act
        var result = await TryExceptAsync.BindAsync(
            () => input,
            [ExcludeFromCodeCoverage] (x) => Task.FromResult(Result<int>.Success(int.Parse(x))));

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task MapAsync_WithValidValue_ReturnsSuccess()
    {
        // arrange
        var input = Task.FromResult(42);

        // act
        var result = await TryExceptAsync.MapAsync(() => input, v => Task.FromResult(v * 2));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(84);
    }

    [TestMethod]
    public async Task MapAsync_WithExceptionThrown_ReturnsFailure()
    {
        // arrange
        var input = Task.FromResult(42);

        // act
        var result = await TryExceptAsync.MapAsync(() => input, v => Task.FromResult(v / 0));

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task RunActionAsync_WithSuccessfulOperation_ReturnsSuccess()
    {
        // arrange
        Result<int>? value = null;
        Func<Task> op = () => { value = Result<int>.Success(42); return Task.CompletedTask; };

        // act
        await TryExceptAsync.RunAsync(
            () => op(),
            [ExcludeFromCodeCoverage] (ex) => Result<int>.Failure(ex));

        // assert
        value.Should().NotBeNull();
        value!.IsSuccess.Should().BeTrue();
        value.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task RunActionAsync_WithOperationException_ReturnsFailure()
    {
        // arrange
        Result<int>? value = null;
        Func<Task> op = () => throw new InvalidOperationException();

        // act
        await TryExceptAsync.RunAsync(
            [ExcludeFromCodeCoverage] () => op(),
            ex => value = Result<int>.Failure(ex));

        // assert
        value.Should().NotBeNull();
        value!.IsFailure.Should().BeTrue();
        value.GetErrors().Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task RunActionAsync_WithSuccessfulOperation_RunsFinally()
    {
        // arrange
        bool ranFinally = false;
        Result<int>? value = null;
        Func<Task> op = () => { value = Result<int>.Success(42); return Task.CompletedTask; };

        // act
        await TryExceptAsync.RunAsync(
            () => op(),
            [ExcludeFromCodeCoverage] (ex) => Result<int>.Failure(ex),
            () => ranFinally = true);

        // assert
        value.Should().NotBeNull();
        value!.IsSuccess.Should().BeTrue();
        value.GetValue().Should().Be(42);
        ranFinally.Should().BeTrue();
    }
}

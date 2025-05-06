using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class ResultMatchAsyncTests
{
    [TestMethod]
    public async Task MatchAsync_WithTask_ReturnsSuccess()
    {
        // arrange
        var task = Task.FromResult(Result<string>.Success("42"));

        // act
        var result = await task.MatchAsync(
            v => Task.FromResult(42),
            [ExcludeFromCodeCoverage] (e) => Task.FromResult(0));

        // assert
        result.Should().Be(42);
    }

    [TestMethod]
    public async Task MatchAsync_WithAsyncSuccess_ReturnsSuccess()
    {
        // arrange
        var r = Result<string>.Success("42");

        // act
        var result = await r.MatchAsync(
            v => ResultHelper.TryParseAsync(v),
            [ExcludeFromCodeCoverage] (e) => Task.FromResult(Result<int>.Success(0)));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task MatchAsync_WithAsyncSuccess_ReturnsFailure()
    {
        // arrange
        var r = Result<string>.Failure(Error.Conflict("test", "message"));

        // act
        var result = await r.MatchAsync(
            [ExcludeFromCodeCoverage] (v) => ResultHelper.TryParseAsync(v),
            e => Task.FromResult(Result<int>.Failure(e)));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task MatchAsync_WithAsyncOnFailure_ReturnsFailure()
    {
        // arrange
        var r = Result<string>.Failure(Error.Conflict("test", "message"));

        // act
        var result = await r.MatchAsync(
            [ExcludeFromCodeCoverage] (v) => Task.FromResult(Result<int>.Success(42)),
            e => Task.FromResult(Result<int>.Failure(e)));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task MatchAsync_WithAsyncOnFailure_ReturnsSuccess()
    {
        // arrange
        var r = Result<string>.Success("42");

        // act
        var result = await r.MatchAsync(
            v => Task.FromResult(Result<int>.Success(42)),
            [ExcludeFromCodeCoverage] (e) => Task.FromResult(Result<int>.Failure(e)));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task MatchAsync_WithAsyncForBoth_ReturnsValue()
    {
        // arrange
        var task = Task.FromResult(Result<string>.Success("42"));

        // act
        var result = await task.MatchAsync(
            v => Task.FromResult(42),
            [ExcludeFromCodeCoverage](e) => Task.FromResult(0));

        // assert
        result.Should().Be(42);
    }

    [TestMethod]
    public async Task MatchAsync_WithAsyncTaskAndOnSuccess_ReturnsSuccess()
    {
        // arrange
        var task = Task.FromResult(Result<string>.Success("42"));

        // act
        var result = await task.MatchAsync(
            v => ResultHelper.TryParseAsync(v), 
            [ExcludeFromCodeCoverage] (e) => Task.FromResult(Result<int>.Success(0)));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task MatchAsync_WithAsyncTaskAndOnFailure_ReturnsFailure()
    {
        // arrange
        var r = Task.FromResult(Result<string>.Failure(Error.Conflict("test", "message")));

        // act
        var result = await r.MatchAsync(
            [ExcludeFromCodeCoverage] (v) => Task.FromResult(Result<int>.Success(42)),
            e => Task.FromResult(Result<int>.Failure(e)));

        // assert
        result.IsFailure.Should().BeTrue();
    }
}

using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class ResultBindMapAsyncTests
{
    [TestMethod]
    public async Task BindAsync_WithTask_ReturnsSuccess()
    {
        // arrange
        var task = Task.FromResult(Result<string>.Success("42"));

        // act
        var result = await task.BindAsync(v => Task.FromResult(Result<int>.Success(42)));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithTaskAndBindTask_ReturnsSuccess()
    {
        // arrange
        var task = Task.FromResult(Result<string>.Success("42"));

        // act
        var result = await task.BindAsync(v => ResultHelper.TryParseAsync(v));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithValidValue_ReturnsSuccess()
    {
        // arrange
        var input = Result<string>.Success("42");

        // act
        var result = await input.BindAsync(v => ResultHelper.TryParseAsync(v));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsFailure()
    {
        // arrange
        var input = Result<string>.Success("non-int-text");

        // act
        var result = await input.BindAsync(v => ResultHelper.TryParseAsync(v));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task BindAsync_WithFailure_ReturnsFailure()
    {
        // arrange
        var input = Result<string>.Failure(Error.Unexpected("Error", "Test error"));

        // act
        var result = await input.BindAsync([ExcludeFromCodeCoverage] (v) => ResultHelper.TryParseAsync(v));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task BindAsync_WithTaskAndFailure_ReturnsFailure()
    {
        // arrange
        var input = Task.FromResult(Result<string>.Failure(Error.Unexpected("Error", "Test error")));

        // act
        var result = await input.BindAsync([ExcludeFromCodeCoverage] (v) => ResultHelper.TryParseAsync(v));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task MapAsync_WithTask_ReturnsSuccess()
    {
        // arrange
        var task = Task.FromResult(Result<int>.Success(42));

        // act
        var result = await task.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(84);
    }

    [TestMethod]
    public async Task MapAsync_WithTaskAndBindTask_ReturnsSuccess()
    {
        // arrange
        var task = Task.FromResult(Result<int>.Success(42));

        // act
        var result = await task.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(84);
    }

    [TestMethod]
    public async Task MapAsync_WithValidValue_ReturnsSuccess()
    {
        // arrange
        var input = Result<int>.Success(42);

        // act
        var result = await input.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Be(84);
    }

    [TestMethod]
    public async Task MapAsync_WithFailure_ReturnsFailure()
    {
        // arrange
        var input = Result<int>.Failure(Error.Invalid("code", "test"));

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskMapAsync_WithFailure_ReturnsFailure()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Failure(Error.Invalid("code", "test")));

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsFailure.Should().BeTrue();
    }

    [TestMethod]
    public async Task MapErrorsAsync_WithFailure_ReturnsNewFailure()
    {
        // arrange
        var input = Task.FromResult(Result<int>.Failure(Error.Invalid("code", "test")));

        // act
        var result = await input.MapErrorsAsync<int, string>();

        // assert
        result.IsFailure.Should().BeTrue();
    }
}

using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class ResultCombineAsyncTests
{
    private static readonly Error _error1 = Error.Validation("Test.Error1", "First error.");
    private static readonly Error _error2 = Error.Validation("Test.Error2", "Second error.");

    private static Task<Result<T>> SuccessAsync<T>(T value) where T : notnull =>
        Task.FromResult(Result<T>.Success(value));

    private static Task<Result<T>> FailureAsync<T>(Error error) where T : notnull =>
        Task.FromResult(Result<T>.Failure(error));

    [TestMethod]
    public async Task CombineAsync_WithAllSuccesses_ReturnsSuccessWithAllValues()
    {
        // arrange
        var results = new[] { SuccessAsync(1), SuccessAsync(2), SuccessAsync(3) };

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(results);

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Equal(1, 2, 3);
    }

    [TestMethod]
    public async Task CombineAsync_WithFailures_CollectsAllErrors()
    {
        // arrange
        var results = new[] { SuccessAsync(1), FailureAsync<int>(_error1), FailureAsync<int>(_error2) };

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(results);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task CombineAsync_WithNullResults_ThrowsArgumentNullException()
    {
        // act and assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => ResultCombineAsyncExtensions.CombineAsync<int>(null!));
    }

    [TestMethod]
    public async Task CombineAsync_WithTwoSuccesses_ProjectsCombinedValue()
    {
        // arrange

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(
            SuccessAsync(1), SuccessAsync("a"), (a, b) => $"{a}{b}");

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be("1a");
    }

    [TestMethod]
    public async Task CombineAsync_WithTwoResultsAndFailures_CollectsErrors()
    {
        // arrange

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(
            FailureAsync<int>(_error1),
            FailureAsync<int>(_error2),
            [ExcludeFromCodeCoverage] (a, b) => a + b);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task CombineAsync_WithTwoResultsAndNullTask_ThrowsArgumentNullException()
    {
        // act and assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => ResultCombineAsyncExtensions.CombineAsync(null!, SuccessAsync(1), [ExcludeFromCodeCoverage](int a, int b) => a + b));
    }

    [TestMethod]
    public async Task CombineAsync_WithThreeSuccesses_ProjectsCombinedValue()
    {
        // arrange

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(
            SuccessAsync(1), SuccessAsync(2), SuccessAsync(3), (a, b, c) => a + b + c);

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be(6);
    }

    [TestMethod]
    public async Task CombineAsync_WithThreeResultsAndFailures_CollectsErrors()
    {
        // arrange

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(
            SuccessAsync(1),
            FailureAsync<int>(_error1),
            FailureAsync<int>(_error2),
            [ExcludeFromCodeCoverage] (a, b, c) => a + b + c);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task CombineAsync_WithThreeResultsAndNullTask_ThrowsArgumentNullException()
    {
        // act and assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => ResultCombineAsyncExtensions.CombineAsync(
                SuccessAsync(1), SuccessAsync(2), null!, [ExcludeFromCodeCoverage](int a, int b, int c) => a + b + c));
    }

    [TestMethod]
    public async Task CombineAsync_WithFourSuccesses_ProjectsCombinedValue()
    {
        // arrange

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(
            SuccessAsync(1), SuccessAsync(2), SuccessAsync(3), SuccessAsync(4), (a, b, c, d) => a + b + c + d);

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be(10);
    }

    [TestMethod]
    public async Task CombineAsync_WithFourResultsAndFailures_CollectsErrors()
    {
        // arrange

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(
            SuccessAsync(1),
            SuccessAsync(2),
            FailureAsync<int>(_error1),
            FailureAsync<int>(_error2),
            [ExcludeFromCodeCoverage] (a, b, c, d) => a + b + c + d);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task CombineAsync_WithFourResultsAndNullTask_ThrowsArgumentNullException()
    {
        // act and assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => ResultCombineAsyncExtensions.CombineAsync(
                SuccessAsync(1),
                SuccessAsync(2),
                SuccessAsync(3),
                null!,
                [ExcludeFromCodeCoverage](int a, int b, int c, int d) => a + b + c + d));
    }

    [TestMethod]
    public async Task CombineAsync_WithFiveSuccesses_ProjectsCombinedValue()
    {
        // arrange

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(
            SuccessAsync(1),
            SuccessAsync(2),
            SuccessAsync(3),
            SuccessAsync(4),
            SuccessAsync(5),
            (a, b, c, d, e) => a + b + c + d + e);

        // assert
        combined.IsSuccess.Should().BeTrue();
        combined.GetValue().Should().Be(15);
    }

    [TestMethod]
    public async Task CombineAsync_WithFiveResultsAndFailures_CollectsErrors()
    {
        // arrange

        // act
        var combined = await ResultCombineAsyncExtensions.CombineAsync(
            SuccessAsync(1),
            SuccessAsync(2),
            SuccessAsync(3),
            FailureAsync<int>(_error1),
            FailureAsync<int>(_error2),
            [ExcludeFromCodeCoverage] (a, b, c, d, e) => a + b + c + d + e);

        // assert
        combined.IsFailure.Should().BeTrue();
        combined.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task CombineAsync_WithFiveResultsAndNullTask_ThrowsArgumentNullException()
    {
        // act and assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => ResultCombineAsyncExtensions.CombineAsync(
                SuccessAsync(1),
                SuccessAsync(2),
                SuccessAsync(3),
                SuccessAsync(4),
                null!,
                [ExcludeFromCodeCoverage](int a, int b, int c, int d, int e) => a + b + c + d + e));
    }

    [TestMethod]
    public async Task TraverseAsync_WithAllSuccesses_ReturnsSuccessWithMappedValues()
    {
        // arrange
        var source = new[] { 1, 2, 3 };

        // act
        var result = await ResultCombineAsyncExtensions.TraverseAsync(source, x => SuccessAsync(x * 2));

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Equal(2, 4, 6);
    }

    [TestMethod]
    public async Task TraverseAsync_WithFailures_CollectsAllErrors()
    {
        // arrange
        var source = new[] { 1, 2, 3 };

        // act
        var result = await ResultCombineAsyncExtensions.TraverseAsync(
            source,
            x => x switch
            {
                2 => FailureAsync<int>(_error1),
                3 => FailureAsync<int>(_error2),
                _ => SuccessAsync(x),
            });

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task TraverseAsync_WithNullSource_ThrowsArgumentNullException()
    {
        // act and assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => ResultCombineAsyncExtensions.TraverseAsync(null!, [ExcludeFromCodeCoverage](int x) => SuccessAsync(x)));
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task TraverseAsync_WithNullSelector_ThrowsArgumentNullException()
    {
        // act and assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => ResultCombineAsyncExtensions.TraverseAsync<int, int>([1, 2], null!));
    }

    [TestMethod]
    public async Task SequenceAsync_WithAllSuccesses_ReturnsSuccessWithAllValues()
    {
        // arrange
        var source = new List<Task<Result<int>>> { SuccessAsync(1), SuccessAsync(2) };

        // act
        var result = await ResultCombineAsyncExtensions.SequenceAsync(source);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.GetValue().Should().Equal(1, 2);
    }

    [TestMethod]
    public async Task SequenceAsync_WithFailures_CollectsAllErrors()
    {
        // arrange
        var source = new List<Task<Result<int>>>
        {
            FailureAsync<int>(_error1),
            SuccessAsync(2),
            FailureAsync<int>(_error2),
        };

        // act
        var result = await ResultCombineAsyncExtensions.SequenceAsync(source);

        // assert
        result.IsFailure.Should().BeTrue();
        result.GetErrors().Should().Equal(_error1, _error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task SequenceAsync_WithNullSource_ThrowsArgumentNullException()
    {
        // act and assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            () => ResultCombineAsyncExtensions.SequenceAsync<int>(null!));
    }
}

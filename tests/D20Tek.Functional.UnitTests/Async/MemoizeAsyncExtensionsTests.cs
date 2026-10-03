using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class MemoizeAsyncExtensionsTests
{
    [TestMethod]
    public async Task MemoizeAsync_WithSingleArg_CachesResultForSameInput()
    {
        // arrange
        int callCount = 0;
        Func<int, Task<int>> func = async x =>
        {
            Interlocked.Increment(ref callCount);
            await Task.Delay(1, CancellationToken.None);
            return x * 2;
        };
        var memoized = func.MemoizeAsync();

        // act
        var result1 = await memoized(5);
        var result2 = await memoized(5);

        // assert
        result1.Should().Be(10);
        result2.Should().Be(10);
        callCount.Should().Be(1);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithSingleArg_RecomputesForDifferentInput()
    {
        // arrange
        int callCount = 0;
        Func<int, Task<int>> func = async x =>
        {
            Interlocked.Increment(ref callCount);
            await Task.Delay(1, CancellationToken.None);
            return x * 2;
        };
        var memoized = func.MemoizeAsync();

        // act
        var result1 = await memoized(5);
        var result2 = await memoized(6);

        // assert
        result1.Should().Be(10);
        result2.Should().Be(12);
        callCount.Should().Be(2);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithSingleArg_SharesInFlightTaskUnderConcurrentAccess()
    {
        // arrange
        int callCount = 0;
        Func<int, Task<int>> func = async x =>
        {
            Interlocked.Increment(ref callCount);
            await Task.Delay(20, CancellationToken.None);
            return x * 2;
        };
        var memoized = func.MemoizeAsync();

        // act
        var tasks = Enumerable.Range(0, 20).Select(_ => memoized(7));
        var results = await Task.WhenAll(tasks);

        // assert
        results.Should().OnlyContain(r => r == 14);
        callCount.Should().Be(1);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithSingleArg_EvictsFailedResultAndAllowsRetry()
    {
        // arrange
        int callCount = 0;
        Func<int, Task<int>> func = async x =>
        {
            callCount++;
            await Task.Delay(1, CancellationToken.None);
            if (callCount == 1)
            {
                throw new InvalidOperationException("first call fails");
            }

            return x * 2;
        };
        var memoized = func.MemoizeAsync();

        // act
        Func<Task> act = async () => await memoized(5);
        await act.Should().ThrowExactlyAsync<InvalidOperationException>();
        var result = await memoized(5);

        // assert
        result.Should().Be(10);
        callCount.Should().Be(2);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithTwoArgs_CachesResultForSameInputs()
    {
        // arrange
        int callCount = 0;
        Func<int, int, Task<int>> func = async (x, y) =>
        {
            callCount++;
            await Task.Delay(1, CancellationToken.None);
            return x + y;
        };
        var memoized = func.MemoizeAsync();

        // act
        var result1 = await memoized(2, 3);
        var result2 = await memoized(2, 3);

        // assert
        result1.Should().Be(5);
        result2.Should().Be(5);
        callCount.Should().Be(1);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithTwoArgs_RecomputesForDifferentInputs()
    {
        // arrange
        int callCount = 0;
        Func<int, int, Task<int>> func = async (x, y) =>
        {
            callCount++;
            await Task.Delay(1, CancellationToken.None);
            return x + y;
        };
        var memoized = func.MemoizeAsync();

        // act
        var result1 = await memoized(2, 3);
        var result2 = await memoized(3, 2);

        // assert
        result1.Should().Be(5);
        result2.Should().Be(5);
        callCount.Should().Be(2);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithTwoArgs_EvictsFailedResultAndAllowsRetry()
    {
        // arrange
        int callCount = 0;
        Func<int, int, Task<int>> func = async (x, y) =>
        {
            callCount++;
            await Task.Delay(1, CancellationToken.None);
            if (callCount == 1)
            {
                throw new InvalidOperationException("first call fails");
            }

            return x + y;
        };
        var memoized = func.MemoizeAsync();

        // act
        Func<Task> act = async () => await memoized(2, 3);
        await act.Should().ThrowExactlyAsync<InvalidOperationException>();
        var result = await memoized(2, 3);

        // assert
        result.Should().Be(5);
        callCount.Should().Be(2);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithThreeArgs_CachesResultForSameInputs()
    {
        // arrange
        int callCount = 0;
        Func<int, int, int, Task<int>> func = async (x, y, z) =>
        {
            callCount++;
            await Task.Delay(1, CancellationToken.None);
            return x + y + z;
        };
        var memoized = func.MemoizeAsync();

        // act
        var result1 = await memoized(1, 2, 3);
        var result2 = await memoized(1, 2, 3);

        // assert
        result1.Should().Be(6);
        result2.Should().Be(6);
        callCount.Should().Be(1);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithThreeArgs_RecomputesForDifferentInputs()
    {
        // arrange
        int callCount = 0;
        Func<int, int, int, Task<int>> func = async (x, y, z) =>
        {
            callCount++;
            await Task.Delay(1, CancellationToken.None);
            return x + y + z;
        };
        var memoized = func.MemoizeAsync();

        // act
        var result1 = await memoized(1, 2, 3);
        var result2 = await memoized(1, 2, 4);

        // assert
        result1.Should().Be(6);
        result2.Should().Be(7);
        callCount.Should().Be(2);
    }

    [TestMethod]
    public async Task MemoizeAsync_WithThreeArgs_EvictsFailedResultAndAllowsRetry()
    {
        // arrange
        int callCount = 0;
        Func<int, int, int, Task<int>> func = async (x, y, z) =>
        {
            callCount++;
            await Task.Delay(1, CancellationToken.None);
            if (callCount == 1)
            {
                throw new InvalidOperationException("first call fails");
            }

            return x + y + z;
        };
        var memoized = func.MemoizeAsync();

        // act
        Func<Task> act = async () => await memoized(1, 2, 3);
        await act.Should().ThrowExactlyAsync<InvalidOperationException>();
        var result = await memoized(1, 2, 3);

        // assert
        result.Should().Be(6);
        callCount.Should().Be(2);
    }
}

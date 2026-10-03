namespace D20Tek.Functional.UnitTests;

[TestClass]
public class MemoizeExtensionsTests
{
    [TestMethod]
    public void Memoize_WithSingleArg_CachesResultForSameInput()
    {
        // arrange
        int callCount = 0;
        Func<int, int> func = x =>
        {
            callCount++;
            return x * 2;
        };
        var memoized = func.Memoize();

        // act
        var result1 = memoized(5);
        var result2 = memoized(5);

        // assert
        result1.Should().Be(10);
        result2.Should().Be(10);
        callCount.Should().Be(1);
    }

    [TestMethod]
    public void Memoize_WithSingleArg_RecomputesForDifferentInput()
    {
        // arrange
        int callCount = 0;
        Func<int, int> func = x =>
        {
            callCount++;
            return x * 2;
        };
        var memoized = func.Memoize();

        // act
        var result1 = memoized(5);
        var result2 = memoized(6);

        // assert
        result1.Should().Be(10);
        result2.Should().Be(12);
        callCount.Should().Be(2);
    }

    [TestMethod]
    public void Memoize_WithSingleArg_IsThreadSafeUnderConcurrentAccess()
    {
        // arrange
        int callCount = 0;
        Func<int, int> func = x =>
        {
            Interlocked.Increment(ref callCount);
            Thread.Sleep(10);
            return x * 2;
        };
        var memoized = func.Memoize();

        // act
        Parallel.For(0, 20, _ => memoized(7));

        // assert
        callCount.Should().Be(1);
    }

    [TestMethod]
    public void Memoize_WithTwoArgs_CachesResultForSameInputs()
    {
        // arrange
        int callCount = 0;
        Func<int, int, int> func = (x, y) =>
        {
            callCount++;
            return x + y;
        };
        var memoized = func.Memoize();

        // act
        var result1 = memoized(2, 3);
        var result2 = memoized(2, 3);

        // assert
        result1.Should().Be(5);
        result2.Should().Be(5);
        callCount.Should().Be(1);
    }

    [TestMethod]
    public void Memoize_WithTwoArgs_RecomputesForDifferentInputs()
    {
        // arrange
        int callCount = 0;
        Func<int, int, int> func = (x, y) =>
        {
            callCount++;
            return x + y;
        };
        var memoized = func.Memoize();

        // act
        var result1 = memoized(2, 3);
        var result2 = memoized(3, 2);

        // assert
        result1.Should().Be(5);
        result2.Should().Be(5);
        callCount.Should().Be(2);
    }

    [TestMethod]
    public void Memoize_WithThreeArgs_CachesResultForSameInputs()
    {
        // arrange
        int callCount = 0;
        Func<int, int, int, int> func = (x, y, z) =>
        {
            callCount++;
            return x + y + z;
        };
        var memoized = func.Memoize();

        // act
        var result1 = memoized(1, 2, 3);
        var result2 = memoized(1, 2, 3);

        // assert
        result1.Should().Be(6);
        result2.Should().Be(6);
        callCount.Should().Be(1);
    }

    [TestMethod]
    public void Memoize_WithThreeArgs_RecomputesForDifferentInputs()
    {
        // arrange
        int callCount = 0;
        Func<int, int, int, int> func = (x, y, z) =>
        {
            callCount++;
            return x + y + z;
        };
        var memoized = func.Memoize();

        // act
        var result1 = memoized(1, 2, 3);
        var result2 = memoized(1, 2, 4);

        // assert
        result1.Should().Be(6);
        result2.Should().Be(7);
        callCount.Should().Be(2);
    }
}

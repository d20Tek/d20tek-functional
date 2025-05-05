using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class OptionAsyncTests
{
    [TestMethod]
    public async Task DefaultWithAsync_WithSomeValue_ReturnsValue()
    {
        // arrange
        var x = 10;
        var option = Option<int>.Some(x);

        // act
        var result = await option.DefaultWithAsync([ExcludeFromCodeCoverage]() => Task.FromResult(x + 1));

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public async Task TaskDefaultWithAsync_WithSomeValue_ReturnsValue()
    {
        // arrange
        var x = 10;
        var option = Task.FromResult(Option<int>.Some(x));

        // act
        var result = await option.DefaultWithAsync([ExcludeFromCodeCoverage] () => Task.FromResult(x + 1));

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public async Task DefaultWithAsync_WithNone_ReturnsDefault()
    {
        // arrange
        var defDate = DateTimeOffset.Now;
        var option = Option<DateTimeOffset>.None();

        // act
        var result = await option.DefaultWithAsync(() => Task.FromResult(defDate));

        // assert
        result.Should().Be(defDate);
    }

    [TestMethod]
    public async Task TaskDefaultWithAsync_WithNone_ReturnsDefault()
    {
        // arrange
        var defDate = DateTimeOffset.Now;
        var option = Task.FromResult(Option<DateTimeOffset>.None());

        // act
        var result = await option.DefaultWithAsync(() => Task.FromResult(defDate));

        // assert
        result.Should().Be(defDate);
    }

    [TestMethod]
    public async Task ExistsAsync_WithSomeAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var option = Option<int>.Some(42);

        // act
        var result = await option.ExistsAsync(x => Task.FromResult(x > 10));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskExistsAsync_WithSomeAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(42));

        // act
        var result = await option.ExistsAsync(x => Task.FromResult(x > 10));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task ExistsAsync_WithSomeAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var option = Option<int>.Some(42);

        // act
        var result = await option.ExistsAsync(x => Task.FromResult(x == 10));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task TaskExistsAsync_WithSomeAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(42));

        // act
        var result = await option.ExistsAsync(x => Task.FromResult(x == 10));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task ExistsAsync_WithNone_ReturnsFalse()
    {
        // arrange
        var option = Option<string>.None();

        // act
        var result = await option.ExistsAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x == string.Empty));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task TaskExistsAsync_WithNone_ReturnsFalse()
    {
        // arrange
        var option = Task.FromResult(Option<string>.None());

        // act
        var result = await option.ExistsAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x == string.Empty));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task FilterAsync_WithSomeAndPositivePredicate_ReturnsSome()
    {
        // arrange
        var option = Option<int>.Some(42);

        // act
        var result = await option.FilterAsync(x => Task.FromResult(x >= 5));

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public async Task TaskFilterAsync_WithSomeAndPositivePredicate_ReturnsSome()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(42));

        // act
        var result = await option.FilterAsync(x => Task.FromResult(x >= 5));

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public async Task FilterAsync_WithSomeAndNegativePredicate_ReturnsNone()
    {
        // arrange
        var option = Option<int>.Some(4);

        // act
        var result = await option.FilterAsync(x => Task.FromResult(x >= 5));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskFilterAsync_WithSomeAndNegativePredicate_ReturnsNone()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(4));

        // act
        var result = await option.FilterAsync(x => Task.FromResult(x >= 5));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task FilterAsync_WithNone_ReturnsNone()
    {
        // arrange
        var option = Option<int>.None();

        // act
        var result = await option.FilterAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x >= 5));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskFilterAsync_WithNone_ReturnsNone()
    {
        // arrange
        var option = Task.FromResult(Option<int>.None());

        // act
        var result = await option.FilterAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x >= 5));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task FoldAsync_WithSomeValue_ReturnsAccumulated()
    {
        // arrange
        var option = Option<int>.Some(1);

        // act
        var result = await option.FoldAsync(0, (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task TaskFoldAsync_WithSomeValue_ReturnsAccumulated()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(1));

        // act
        var result = await option.FoldAsync(0, (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task FoldAsync_WithSomeValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var option = Option<int>.Some(2);

        // act
        var result = await option.FoldAsync(10, (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public async Task FoldAsync_WithNone_ReturnsInitial()
    {
        // arrange
        var option = Option<int>.None();

        // act
        var result = await option.FoldAsync(
            0, [ExcludeFromCodeCoverage] (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(0);
    }

    [TestMethod]
    public async Task TaskFoldAsync_WithNone_ReturnsInitial()
    {
        // arrange
        var option = Task.FromResult(Option<int>.None());

        // act
        var result = await option.FoldAsync(
            0, [ExcludeFromCodeCoverage] (acc, value) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(0);
    }

    [TestMethod]
    public async Task FoldBackAsync_WithSomeValue_ReturnsAccumulated()
    {
        // arrange
        var option = Option<int>.Some(1);

        // act
        var result = await option.FoldBackAsync(0, (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task TaskFoldBackAsync_WithSomeValue_ReturnsAccumulated()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(1));

        // act
        var result = await option.FoldBackAsync(0, (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task FoldBackAsync_WithSomeValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var option = Option<int>.Some(2);

        // act
        var result = await option.FoldBackAsync(10, (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public async Task FoldBackAsync_WithNone_ReturnsInitial()
    {
        // arrange
        var option = Option<int>.None();

        // act
        var result = await option.FoldBackAsync(
            0, [ExcludeFromCodeCoverage] (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(0);
    }

    [TestMethod]
    public async Task TaskFoldBackAsync_WithNone_ReturnsInitial()
    {
        // arrange
        var option = Task.FromResult(Option<int>.None());

        // act
        var result = await option.FoldBackAsync(
            0, [ExcludeFromCodeCoverage] (value, acc) => Task.FromResult(acc + value * 2));

        // assert
        result.Should().Be(0);
    }

    [TestMethod]
    public async Task ForAllAsync_WithSomeAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var option = Option<int>.Some(42);

        // act
        var result = await option.ForAllAsync(x => Task.FromResult(x >= 5));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskForAllAsync_WithSomeAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(42));

        // act
        var result = await option.ForAllAsync(x => Task.FromResult(x >= 5));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task ForAllAsync_WithSomeAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var option = Option<int>.Some(4);

        // act
        var result = await option.ForAllAsync(x => Task.FromResult(x >= 5));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task TaskForAllAsync_WithSomeAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(4));

        // act
        var result = await option.ForAllAsync(x => Task.FromResult(x >= 5));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task ForAll_WithNone_ReturnsTrue()
    {
        // arrange
        var option = Option<string>.None();

        // act
        var result = await option.ForAllAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x == string.Empty));
        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskForAll_WithNone_ReturnsTrue()
    {
        // arrange
        var option = Task.FromResult(Option<string>.None());

        // act
        var result = await option.ForAllAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(x == string.Empty));
        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task IterAsync_WithSomeValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var option = Option<string>.Some("Hello World");

        // act
        await option.IterAsync(x => Task.FromResult(output = x));

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public async Task TaskIterAsync_WithSomeValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var option = Task.FromResult(Option<string>.Some("Hello World"));

        // act
        await option.IterAsync(x => Task.FromResult(output = x));

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public async Task IterAsync_WithNone_DoesNotPerformAction()
    {
        // arrange
        var output = string.Empty;
        var option = Option<string>.None();

        // act
        await option.IterAsync([ExcludeFromCodeCoverage] (x) => Task.FromResult(output = x));

        // assert
        output.Should().Be(string.Empty);
    }

    [TestMethod]
    public async Task OrElseWithAsync_WithSomeValue_ReturnsThatSome()
    {
        // arrange
        var option = Option<decimal>.Some(100M);
        var ifNone = Option<decimal>.Some(41);

        // act
        var result = await option.OrElseWithAsync([ExcludeFromCodeCoverage] () => Task.FromResult(ifNone));

        // assert
        result.Should().Be(option);
        result.Get().Should().Be(100M);
    }

    [TestMethod]
    public async Task TaskOrElseWithAsync_WithSomeValue_ReturnsThatSome()
    {
        // arrange
        var option = Task.FromResult(Option<decimal>.Some(100M));
        var ifNone = Option<decimal>.Some(41);

        // act
        var result = await option.OrElseWithAsync([ExcludeFromCodeCoverage] () => Task.FromResult(ifNone));

        // assert
        result.Should().Be(option.Result);
        result.Get().Should().Be(100M);
    }

    [TestMethod]
    public async Task OrElseWithAsync_WithNoneValue_ReturnsOtherSome()
    {
        // arrange
        var option = Option<decimal>.None();
        var ifNone = Option<decimal>.Some(41);

        // act
        var result = await option.OrElseWithAsync(() => Task.FromResult(ifNone));

        // assert
        result.Should().Be(ifNone);
        result.Get().Should().Be(41);
    }

    [TestMethod]
    public async Task TaskOrElseWithAsync_WithNoneValue_ReturnsOtherSome()
    {
        // arrange
        var option = Task.FromResult(Option<decimal>.None());
        var ifNone = Option<decimal>.Some(41);

        // act
        var result = await option.OrElseWithAsync(() => Task.FromResult(ifNone));

        // assert
        result.Should().Be(ifNone);
        result.Get().Should().Be(41);
    }
}
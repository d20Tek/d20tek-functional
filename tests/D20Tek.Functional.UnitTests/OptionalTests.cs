namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionalTests
{
    [TestMethod]
    public void IsSome_WithSomeValue_ReturnsTrue()
    {
        // arrange
        var option = Optional<int>.Some(13);

        // act
        var isSome = option.IsSome;
        var isNone = option.IsNone;

        // assert
        isSome.Should().BeTrue();
        isNone.Should().BeFalse();
    }

    [TestMethod]
    public void IsNone_WithNoValue_ReturnsTrue()
    {
        // arrange
        var option = Optional<string>.None();

        // act
        var isNone = option.IsNone;
        var isSome = option.IsSome;

        // assert
        isNone.Should().BeTrue();
        isSome.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_WithEqualSomeValue_ReturnsTrue()
    {
        // arrange
        var option = Optional<string>.Some("foo");

        // act
        bool contains = option.Contains("foo");

        // assert
        contains.Should().BeTrue();
    }


    [TestMethod]
    public void Contains_WithUnequalSomeValue_ReturnsFalse()
    {
        // arrange
        var option = Optional<string>.Some("foo");

        // act
        bool contains = option.Contains("bar");

        // assert
        contains.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_WithNoneValue_ReturnsFalse()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        bool contains = option.Contains(12);

        // assert
        contains.Should().BeFalse();
    }

    [TestMethod]
    public void Count_WithSomeValue_ReturnsOne()
    {
        // arrange
        var option = Optional<DateTimeOffset>.Some(DateTimeOffset.Now);

        // act
        int count = option.Count();

        // assert
        count.Should().Be(1);
    }

    [TestMethod]
    public void Count_WithNoneValue_ReturnsZero()
    {
        // arrange
        var option = Optional<DateTimeOffset>.None();

        // act
        int count = option.Count();

        // assert
        count.Should().Be(0);
    }

    [TestMethod]
    public void DefaultValue_WithSomeValue_ReturnsValue()
    {
        // arrange
        var option = Optional<int>.Some(10);

        // act
        var result = option.DefaultValue(42);

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public void DefaultValue_WithNone_ReturnsDefault()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        var result = option.DefaultValue(42);

        // assert
        result.Should().Be(42);
    }

    [TestMethod]
    public void DefaultWith_WithSomeValue_ReturnsValue()
    {
        // arrange
        var x = 10;
        var option = Optional<int>.Some(x);

        // act
        var result = option.DefaultWith([ExcludeFromCodeCoverage] () => x + 1);

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public void DefaultWith_WithNone_ReturnsDefault()
    {
        // arrange
        var defDate = DateTimeOffset.Now;
        var option = Optional<DateTimeOffset>.None();

        // act
        var result = option.DefaultWith(() => defDate);

        // assert
        result.Should().Be(defDate);
    }

    [TestMethod]
    public void Exists_WithSomeAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var option = Optional<int>.Some(42);

        // act
        var result = option.Exists(x => x > 10);

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Exists_WithSomeAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var option = Optional<int>.Some(42);

        // act
        var result = option.Exists(x => x == 10);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Exists_WithNone_ReturnsFalse()
    {
        // arrange
        var option = Optional<string>.None();

        // act
        var result = option.Exists([ExcludeFromCodeCoverage] (x) => x == string.Empty);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Filter_WithSomeAndPositivePredicate_ReturnsSome()
    {
        // arrange
        var option = Optional<int>.Some(42);

        // act
        var result = option.Filter(x => x >= 5);

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void Filter_WithSomeAndNegativePredicate_ReturnsNone()
    {
        // arrange
        var option = Optional<int>.Some(4);

        // act
        var result = option.Filter(x => x >= 5);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Filter_WithNone_ReturnsNone()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        var result = option.Filter([ExcludeFromCodeCoverage] (x) => x >= 5);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Fold_WithSomeValue_ReturnsAccumulated()
    {
        // arrange
        var option = Optional<int>.Some(1);

        // act
        var result = option.Fold(0, (acc, value) => acc + value * 2);

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public void Fold_WithSomeValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var option = Optional<int>.Some(2);

        // act
        var result = option.Fold(10, (acc, value) => acc + value * 2);

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public void Fold_WithNone_ReturnsInitial()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        var result = option.Fold(0, [ExcludeFromCodeCoverage] (acc, value) => acc + value * 2);

        // assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void FoldBack_WithSomeValue_ReturnsAccumulated()
    {
        // arrange
        var option = Optional<int>.Some(1);

        // act
        var result = option.FoldBack(0, (value, acc) => acc + value * 2);

        // assert
        result.Should().Be(2);
    }

    [TestMethod]
    public void FoldBack_WithSomeValueAndInitial_ReturnsAccumulated()
    {
        // arrange
        var option = Optional<int>.Some(2);

        // act
        var result = option.FoldBack(10, (value, acc) => acc + value * 2);

        // assert
        result.Should().Be(14);
    }

    [TestMethod]
    public void FoldBack_WithNone_ReturnsInitial()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        var result = option.FoldBack(0, [ExcludeFromCodeCoverage] (value, acc) => acc + value * 2);

        // assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void ForAll_WithSomeAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var option = Optional<int>.Some(42);

        // act
        var result = option.ForAll(x => x >= 5);

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void ForAll_WithSomeAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var option = Optional<int>.Some(4);

        // act
        var result = option.ForAll(x => x >= 5);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void ForAll_WithNone_ReturnsTrue()
    {
        // arrange
        var option = Optional<string>.None();

        // act
        var result = option.ForAll([ExcludeFromCodeCoverage] (x) => x == string.Empty);

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Get_WithSomeValue_ReturnsValue()
    {
        // arrange
        var option = Optional<decimal>.Some(100);

        // act
        var result = option.Get();

        // assert
        result.Should().Be(100M);
    }

    [TestMethod]
    public void Get_WithNone_ThrowsException()
    {
        // arrange
        var option = Optional<decimal>.None();

        // act
        Assert.Throws<ArgumentNullException>([ExcludeFromCodeCoverage]() => option.Get());
    }

    [TestMethod]
    public void Iter_WithSomeValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var option = Optional<string>.Some("Hello World");

        // act
        option.Iter(x => output = x);

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public void Iter_WithNone_DoesNotPerformAction()
    {
        // arrange
        var output = string.Empty;
        var option = Optional<string>.None();

        // act
        option.Iter([ExcludeFromCodeCoverage] (x) => output = x);

        // assert
        output.Should().Be(string.Empty);
    }

    [TestMethod]
    public void OrElse_WithSomeValue_ReturnsThatSome()
    {
        // arrange
        var option = Optional<decimal>.Some(100M);
        var ifNone = Optional<decimal>.Some(41);

        // act
        var result = option.OrElse(ifNone);

        // assert
        result.Should().Be(option);
        result.Get().Should().Be(100M);
    }

    [TestMethod]
    public void OrElse_WithNoneValue_ReturnsOtherSome()
    {
        // arrange
        var option = Optional<decimal>.None();
        var ifNone = Optional<decimal>.Some(41);

        // act
        var result = option.OrElse(ifNone);

        // assert
        result.Should().Be(ifNone);
        result.Get().Should().Be(41);
    }

    [TestMethod]
    public void OrElseWith_WithSomeValue_ReturnsThatSome()
    {
        // arrange
        var option = Optional<decimal>.Some(100M);
        var ifNone = Optional<decimal>.Some(41);

        // act
        var result = option.OrElseWith([ExcludeFromCodeCoverage] () => ifNone);

        // assert
        result.Should().Be(option);
        result.Get().Should().Be(100M);
    }

    [TestMethod]
    public void OrElseWith_WithNoneValue_ReturnsOtherSome()
    {
        // arrange
        var option = Optional<decimal>.None();
        var ifNone = Optional<decimal>.Some(41);

        // act
        var result = option.OrElseWith(() => ifNone);

        // assert
        result.Should().Be(ifNone);
        result.Get().Should().Be(41);
    }

    [TestMethod]
    public void opImplicit_WithValue_ReturnsSome()
    {
        // arrange
        Some<string> option = "test";

        // act
        string value = option;

        // assert
        option.IsSome.Should().BeTrue();
        value.Should().Be("test");
    }

    [TestMethod]
    public void opImplicit_WithValue_ReturnsOption()
    {
        // arrange
        Optional<string> option = "test";

        // act

        // assert
        option.IsSome.Should().BeTrue();
        option.Get().Should().Be("test");
    }
}
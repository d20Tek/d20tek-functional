namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionTests
{
    [TestMethod]
    public void IsSome_WithSomeValue_ReturnsTrue()
    {
        // arrange
        var option = Option<int>.Some(13);

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
        var option = Option<string>.None();

        // act
        var isNone = option.IsNone;
        var isSome = option.IsSome;

        // assert
        isNone.Should().BeTrue();
        isSome.Should().BeFalse();
    }

    [TestMethod]
    public void Bind_WithValidValue_ReturnsSome()
    {
        // arrange
        var option = Option<string>.Some("42");

        // act
        var result = option.Bind(v => OptionHelper.TryParse(v));

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsSome()
    {
        // arrange
        var option = Option<string>.Some("non-int-text");

        // act
        var result = option.Bind(v => OptionHelper.TryParse(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Bind_WithNoneOption_ReturnsSome()
    {
        // arrange
        var option = Option<string>.None();

        // act
        var result = option.Bind([ExcludeFromCodeCoverage](v) => OptionHelper.TryParse(v));

        // assert
        result.IsNone.Should().BeTrue();
    }
    [TestMethod]
    public void Contains_WithEqualSomeValue_ReturnsTrue()
    {
        // arrange
        var option = Option<string>.Some("foo");

        // act
        bool contains = option.Contains("foo");

        // assert
        contains.Should().BeTrue();
    }


    [TestMethod]
    public void Contains_WithUnequalSomeValue_ReturnsFalse()
    {
        // arrange
        var option = Option<string>.Some("foo");

        // act
        bool contains = option.Contains("bar");

        // assert
        contains.Should().BeFalse();
    }

    [TestMethod]
    public void Contains_WithNoneValue_ReturnsFalse()
    {
        // arrange
        var option = Option<int>.None();

        // act
        bool contains = option.Contains(12);

        // assert
        contains.Should().BeFalse();
    }

    [TestMethod]
    public void Count_WithSomeValue_ReturnsOne()
    {
        // arrange
        var option = Option<DateTimeOffset>.Some(DateTimeOffset.Now);

        // act
        int count = option.Count();

        // assert
        count.Should().Be(1);
    }

    [TestMethod]
    public void Count_WithNoneValue_ReturnsZero()
    {
        // arrange
        var option = Option<DateTimeOffset>.None();

        // act
        int count = option.Count();

        // assert
        count.Should().Be(0);
    }

    [TestMethod]
    public void DefaultValue_WithSomeValue_ReturnsValue()
    {
        // arrange
        var option = Option<int>.Some(10);

        // act
        var result = option.DefaultValue(42);

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public void DefaultValue_WithNone_ReturnsDefault()
    {
        // arrange
        var option = Option<int>.None();

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
        var option = Option<int>.Some(x);

        // act
        var result = option.DefaultWith([ExcludeFromCodeCoverage]() => x + 1);

        // assert
        result.Should().Be(10);
    }

    [TestMethod]
    public void DefaultWith_WithNone_ReturnsDefault()
    {
        // arrange
        var defDate = DateTimeOffset.Now;
        var option = Option<DateTimeOffset>.None();

        // act
        var result = option.DefaultWith(() => defDate);

        // assert
        result.Should().Be(defDate);
    }

    [TestMethod]
    public void Exists_WithSomeAndPositivePredicate_ReturnsTrue()
    {
        // arrange
        var option = Option<int>.Some(42);

        // act
        var result = option.Exists(x => x > 10);

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Exists_WithSomeAndNegativePredicate_ReturnsFalse()
    {
        // arrange
        var option = Option<int>.Some(42);

        // act
        var result = option.Exists(x => x == 10);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Exists_WithNone_ReturnsFalse()
    {
        // arrange
        var option = Option<string>.None();

        // act
        var result = option.Exists([ExcludeFromCodeCoverage] (x) => x == string.Empty);

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void Filter_WithSomeAndPositivePredicate_ReturnsSome()
    {
        // arrange
        var option = Option<int>.Some(42);

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
        var option = Option<int>.Some(4);

        // act
        var result = option.Filter(x => x >= 5);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Filter_WithNone_ReturnsNone()
    {
        // arrange
        var option = Option<int>.None();

        // act
        var result = option.Filter([ExcludeFromCodeCoverage] (x) => x >= 5);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Get_WithSomeValue_ReturnsValue()
    {
        // arrange
        var option = Option<decimal>.Some(100);

        // act
        var result = option.Get();

        // assert
        result.Should().Be(100M);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Get_WithNone_ThrowsException()
    {
        // arrange
        var option = Option<decimal>.None();

        // act
        _ = option.Get();
    }
}
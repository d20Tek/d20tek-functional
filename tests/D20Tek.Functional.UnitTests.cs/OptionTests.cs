using FluentAssertions;

namespace D20Tek.Functional.UnitTests.cs;

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
        //int result = option.Bind<int>(v => tryParse(v));

        //static Option<int> tryParse(string v)
        //{
        //    bool r = int.TryParse(v, out int parsed);
        //    return r ? Option<int>.Some(parsed) : Option<int>.None();
        //};

        // assert

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
}
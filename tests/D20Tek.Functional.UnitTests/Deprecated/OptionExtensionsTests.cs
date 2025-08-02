namespace D20Tek.Functional.UnitTests.Deprecated;

[TestClass]
[Obsolete("Deprecated - moving to Optional<T> instead.")]
public class OptionExtensionsTests
{
    [TestMethod]
    public void ToOption_WithValue_ReturnsSome()
    {
        // arrange
        var value = 42;

        // act
        var result = value.ToOption();

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void ToOption_WithNull_ReturnsNone()
    {
        // arrange
        string? value = null;

        // act
        var result = value.ToOption();

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Flatten_WithSomeSomeValue_ReturnsSome()
    {
        // arrange
        var option = Option<Option<int>>.Some(Option<int>.Some(42));

        // act
        var result = option.Flatten();

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void Flatten_WithSomeNoneValue_ReturnsNone()
    {
        // arrange
        var option = Option<Option<int>>.Some(Option<int>.None());

        // act
        var result = option.Flatten();

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Flatten_WithNoneNone_ReturnsNone()
    {
        // arrange
        var option = Option<Option<int>>.None();

        // act
        var result = option.Flatten();

        // assert
        result.IsNone.Should().BeTrue();
    }
}

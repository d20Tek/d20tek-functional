namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionalExtensionsTests
{
    [TestMethod]
    public void ToOptional_WithValue_ReturnsSome()
    {
        // arrange
        var value = 42;

        // act
        var result = value.ToOptional();

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void ToOptional_WithNull_ReturnsNone()
    {
        // arrange
        string? value = null;

        // act
        var result = value.ToOptional();

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Flatten_WithSomeSomeValue_ReturnsSome()
    {
        // arrange
        var option = Optional<Optional<int>>.Some(Optional<int>.Some(42));

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
        var option = Optional<Optional<int>>.Some(Optional<int>.None());

        // act
        var result = option.Flatten();

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Flatten_WithNoneNone_ReturnsNone()
    {
        // arrange
        var option = Optional<Optional<int>>.None();

        // act
        var result = option.Flatten();

        // assert
        result.IsNone.Should().BeTrue();
    }
}

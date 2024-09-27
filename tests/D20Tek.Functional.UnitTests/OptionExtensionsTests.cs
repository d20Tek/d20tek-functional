namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionExtensionsTests
{
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

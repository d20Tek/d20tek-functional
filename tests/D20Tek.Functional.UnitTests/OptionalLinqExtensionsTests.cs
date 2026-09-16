namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionalLinqExtensionsTests
{
    [TestMethod]
    public void Select_WithSome_ReturnsProjectedSome()
    {
        // arrange
        var option = Optional<int>.Some(21);

        // act
        var result = option.Select(v => v * 2);

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void Select_WithNone_ReturnsNone()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        var result = option.Select([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Select_WithQuerySyntax_ProjectsValue()
    {
        // arrange
        var option = Optional<int>.Some(10);

        // act
        var result = from v in option
                     select v + 5;

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(15);
    }

    [TestMethod]
    public void SelectMany_WithBothSome_ReturnsCombinedSome()
    {
        // arrange
        var option = Optional<string>.Some("42");

        // act
        var result = option.SelectMany(v => OptionalHelper.TryParse(v), (text, number) => $"{text}:{number}");

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be("42:42");
    }

    [TestMethod]
    public void SelectMany_WithSourceNone_ReturnsNone()
    {
        // arrange
        var option = Optional<string>.None();

        // act
        var result = option.SelectMany(
            [ExcludeFromCodeCoverage] (v) => OptionalHelper.TryParse(v),
            [ExcludeFromCodeCoverage] (text, number) => $"{text}:{number}");

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void SelectMany_WithIntermediateNone_ReturnsNone()
    {
        // arrange
        var option = Optional<string>.Some("non-int-text");

        // act
        var result = option.SelectMany(
            v => OptionalHelper.TryParse(v),
            [ExcludeFromCodeCoverage] (text, number) => $"{text}:{number}");

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void SelectMany_WithQuerySyntax_ComposesTwoOptionals()
    {
        // arrange
        var first = Optional<int>.Some(3);
        var second = Optional<int>.Some(4);

        // act
        var result = from a in first
                     from b in second
                     select a * b;

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(12);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void SelectMany_WithQuerySyntaxAndSecondNone_ReturnsNone()
    {
        // arrange
        var first = Optional<int>.Some(3);
        var second = Optional<int>.None();

        // act
        var result = from a in first
                     from b in second
                     select a * b;

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Where_WithSomeSatisfyingPredicate_ReturnsSome()
    {
        // arrange
        var option = Optional<int>.Some(42);

        // act
        var result = option.Where(v => v > 10);

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void Where_WithSomeFailingPredicate_ReturnsNone()
    {
        // arrange
        var option = Optional<int>.Some(5);

        // act
        var result = option.Where(v => v > 10);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Where_WithNone_ReturnsNone()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        var result = option.Where([ExcludeFromCodeCoverage] (v) => v > 10);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Where_WithQuerySyntaxSatisfyingPredicate_ReturnsSome()
    {
        // arrange
        var option = Optional<int>.Some(42);

        // act
        var result = from v in option
                     where v > 10
                     select v;

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void Where_WithQuerySyntaxFailingPredicate_ReturnsNone()
    {
        // arrange
        var option = Optional<int>.Some(5);

        // act
        var result = from v in option
                     where v > 10
                     select v;

        // assert
        result.IsNone.Should().BeTrue();
    }
}

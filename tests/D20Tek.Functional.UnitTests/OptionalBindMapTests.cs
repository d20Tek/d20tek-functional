namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionalBindMapTests
{
    [TestMethod]
    public void Bind_WithValidValue_ReturnsSome()
    {
        // arrange
        var option = Optional<string>.Some("42");

        // act
        var result = option.Bind(v => OptionalHelper.TryParse(v));

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsNone()
    {
        // arrange
        var option = Optional<string>.Some("non-int-text");

        // act
        var result = option.Bind(v => OptionalHelper.TryParse(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Bind_WithNoneOption_ReturnsNone()
    {
        // arrange
        var option = Optional<string>.None();

        // act
        var result = option.Bind([ExcludeFromCodeCoverage] (v) => OptionalHelper.TryParse(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map_WithValidValue_ReturnsSome()
    {
        // arrange
        var option = Optional<int>.Some(42);

        // act
        var result = option.Map(v => v * 2);

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(84);
    }

    [TestMethod]
    public void Map_WithNoneOption_ReturnsSome()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        var result = option.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map2_WithBothSomeValue_ReturnsSome()
    {
        // arrange
        var option1 = Optional<int>.Some(42);
        var option2 = Optional<decimal>.Some(10M);

        // act
        var result = option1.Map(option2, (v, d) => v * 2 + d);

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(94);
    }

    [TestMethod]
    public void Map2_WithBothNone_ReturnsNone()
    {
        // arrange
        var option1 = Optional<string>.None();
        var option2 = Optional<string>.None();

        // act
        var result = option1.Map(option2, [ExcludeFromCodeCoverage] (s1, s2) => s1 + s2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map2_WithOption1None_ReturnsNone()
    {
        // arrange
        var option1 = Optional<string>.None();
        var option2 = Optional<string>.Some("Test");

        // act
        var result = option1.Map(option2, [ExcludeFromCodeCoverage] (s1, s2) => s1 + s2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map2_WithOption2None_ReturnsNone()
    {
        // arrange
        var option1 = Optional<string>.Some("First");
        var option2 = Optional<string>.None();

        // act
        var result = option1.Map(option2, [ExcludeFromCodeCoverage] (s1, s2) => s1 + s2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map3_WithAllSomeValue_ReturnsSome()
    {
        // arrange
        var option1 = Optional<int>.Some(42);
        var option2 = Optional<decimal>.Some(10M);
        var option3 = Optional<int>.Some(1);

        // act
        var result = option1.Map(option2, option3, (x, y, z) => x + y + z);

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(53);
    }

    [TestMethod]
    public void Map3_WithAllNone_ReturnsNone()
    {
        // arrange
        var option1 = Optional<string>.None();
        var option2 = Optional<string>.None();
        var option3 = Optional<string>.None();

        // act
        var result = option1.Map(option2, option3, [ExcludeFromCodeCoverage] (s1, s2, s3) => s1 + s2 + s3);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map3_WithOption1None_ReturnsNone()
    {
        // arrange
        var option1 = Optional<int>.None();
        var option2 = Optional<decimal>.Some(10M);
        var option3 = Optional<int>.Some(1);

        // act
        var result = option1.Map(option2, option3, [ExcludeFromCodeCoverage] (x, y, z) => x + y + z);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map3_WithOption2None_ReturnsNone()
    {
        // arrange
        var option1 = Optional<int>.Some(42);
        var option2 = Optional<decimal>.None();
        var option3 = Optional<int>.Some(1);

        // act
        var result = option1.Map(option2, option3, [ExcludeFromCodeCoverage] (x, y, z) => x + y + z);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map3_WithOption3None_ReturnsNone()
    {
        // arrange
        var option1 = Optional<int>.Some(42);
        var option2 = Optional<decimal>.Some(10M);
        var option3 = Optional<int>.None();

        // act
        var result = option1.Map(option2, option3, [ExcludeFromCodeCoverage] (x, y, z) => x + y + z);

        // assert
        result.IsNone.Should().BeTrue();
    }
}

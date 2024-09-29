namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionBindMapTests
{
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
    public void Bind_WithInvalidValue_ReturnsNone()
    {
        // arrange
        var option = Option<string>.Some("non-int-text");

        // act
        var result = option.Bind(v => OptionHelper.TryParse(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Bind_WithNoneOption_ReturnsNone()
    {
        // arrange
        var option = Option<string>.None();

        // act
        var result = option.Bind([ExcludeFromCodeCoverage] (v) => OptionHelper.TryParse(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map_WithValidValue_ReturnsSome()
    {
        // arrange
        var option = Option<int>.Some(42);

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
        var option = Option<int>.None();

        // act
        var result = option.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map2_WithBothSomeValue_ReturnsSome()
    {
        // arrange
        var option1 = Option<int>.Some(42);
        var option2 = Option<decimal>.Some(10M);

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
        var option1 = Option<string>.None();
        var option2 = Option<string>.None();

        // act
        var result = option1.Map(option2, [ExcludeFromCodeCoverage] (s1, s2) => s1 + s2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map2_WithOption1None_ReturnsNone()
    {
        // arrange
        var option1 = Option<string>.None();
        var option2 = Option<string>.Some("Test");

        // act
        var result = option1.Map(option2, [ExcludeFromCodeCoverage] (s1, s2) => s1 + s2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map2_WithOption2None_ReturnsNone()
    {
        // arrange
        var option1 = Option<string>.Some("First");
        var option2 = Option<string>.None();

        // act
        var result = option1.Map(option2, [ExcludeFromCodeCoverage] (s1, s2) => s1 + s2);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map3_WithAllSomeValue_ReturnsSome()
    {
        // arrange
        var option1 = Option<int>.Some(42);
        var option2 = Option<decimal>.Some(10M);
        var option3 = Option<int>.Some(1);

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
        var option1 = Option<string>.None();
        var option2 = Option<string>.None();
        var option3 = Option<string>.None();

        // act
        var result = option1.Map(option2, option3, [ExcludeFromCodeCoverage] (s1, s2, s3) => s1 + s2 + s3);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map3_WithOption1None_ReturnsNone()
    {
        // arrange
        var option1 = Option<int>.None();
        var option2 = Option<decimal>.Some(10M);
        var option3 = Option<int>.Some(1);

        // act
        var result = option1.Map(option2, option3, [ExcludeFromCodeCoverage] (x, y, z) => x + y + z);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map3_WithOption2None_ReturnsNone()
    {
        // arrange
        var option1 = Option<int>.Some(42);
        var option2 = Option<decimal>.None();
        var option3 = Option<int>.Some(1);

        // act
        var result = option1.Map(option2, option3, [ExcludeFromCodeCoverage] (x, y, z) => x + y + z);

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public void Map3_WithOption3None_ReturnsNone()
    {
        // arrange
        var option1 = Option<int>.Some(42);
        var option2 = Option<decimal>.Some(10M);
        var option3 = Option<int>.None();

        // act
        var result = option1.Map(option2, option3, [ExcludeFromCodeCoverage] (x, y, z) => x + y + z);

        // assert
        result.IsNone.Should().BeTrue();
    }
}

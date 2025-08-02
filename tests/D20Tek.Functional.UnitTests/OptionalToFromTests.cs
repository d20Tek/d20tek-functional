namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionalToFromTests
{
    [TestMethod]
    public void Some_WithValue_ReturnsSome()
    {
        // arrange
        var option = Optional.Some<int>(13);

        // act
        var isSome = option.IsSome;
        var isNone = option.IsNone;

        // assert
        isSome.Should().BeTrue();
        option.Get().Should().Be(13);
        isNone.Should().BeFalse();
    }

    [TestMethod]
    public void None_WithNoValue_ReturnsNone()
    {
        // arrange
        var option = Optional.None<string>();

        // act
        var isNone = option.IsNone;
        var isSome = option.IsSome;

        // assert
        isNone.Should().BeTrue();
        isSome.Should().BeFalse();
    }

    [TestMethod]
    public void OfNullable_WithValue_ReturnsSome()
    {
        // arrange
        var option = Optional.OfNullable<int>(101);

        // act
        var isSome = option.IsSome;
        var isNone = option.IsNone;

        // assert
        isSome.Should().BeTrue();
        option.Get().Should().Be(101);
        isNone.Should().BeFalse();
    }

    [TestMethod]
    public void OfNullable_WithNull_ReturnsNone()
    {
        // arrange
        var option = Optional.OfNullable<int>(null);

        // act
        var isSome = option.IsSome;
        var isNone = option.IsNone;

        // assert
        isSome.Should().BeFalse();
        isNone.Should().BeTrue();
    }

    [TestMethod]
    public void OfObj_WithValue_ReturnsSome()
    {
        // arrange
        var option = Optional.OfObj<string>("test");

        // act
        var isSome = option.IsSome;
        var isNone = option.IsNone;

        // assert
        isSome.Should().BeTrue();
        option.Get().Should().Be("test");
        isNone.Should().BeFalse();
    }

    [TestMethod]
    public void OfObj_WithNull_ReturnsNone()
    {
        // arrange
        var option = Optional.OfObj<string>(null);

        // act
        var isSome = option.IsSome;
        var isNone = option.IsNone;

        // assert
        isSome.Should().BeFalse();
        isNone.Should().BeTrue();
    }

    [TestMethod]
    public void ToArray_WithSomeValue_ReturnsSingleItemArray()
    {
        // arrange
        var option = Optional<string>.Some("test");

        // act
        var result = option.ToArray();

        // assert
        result.Should().HaveCount(1);
        result.Should().Contain("test");
    }

    [TestMethod]
    public void ToArray_WithNone_ReturnsEmptyArray()
    {
        // arrange
        var option = Optional<string>.None();

        // act
        var result = option.ToArray();

        // assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToList_WithSomeValue_ReturnsSingleItemList()
    {
        // arrange
        var option = Optional<string>.Some("test");

        // act
        var result = option.ToList();

        // assert
        result.Should().HaveCount(1);
        result.Should().Contain("test");
    }

    [TestMethod]
    public void ToList_WithNone_ReturnsEmptyList()
    {
        // arrange
        var option = Optional<string>.None();

        // act
        var result = option.ToList();

        // assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToNullable_WithSomeValue_ReturnsValue()
    {
        // arrange
        var option = Optional<int>.Some(12);

        // act
        int? result = option.ToNullable();

        // assert
        result.Should().NotBeNull();
        result.Should().Be(12);
    }

    [TestMethod]
    public void ToNullable_WithNone_ReturnsDefault()
    {
        // arrange
        var option = Optional<int>.None();

        // act
        int? result = option.ToNullable();

        // assert
        result.Should().NotBeNull();
        result.Should().Be(0);
    }

    [TestMethod]
    public void ToObj_WithSomeValue_ReturnsValue()
    {
        // arrange
        var option = Optional<string>.Some("test");

        // act
        var result = option.ToObj();

        // assert
        result.Should().NotBeNull();
        result.Should().Be("test");
    }

    [TestMethod]
    public void ToObj_WithNone_ReturnsDefault()
    {
        // arrange
        var option = Optional<string>.None();

        // act
        var result = option.ToObj();

        // assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void ToString_WithSomeValue_ReturnsSomeString()
    {
        // arrange
        var option = Optional.Some<int>(13);

        // act
        var result = option.ToString();

        // assert
        result.Should().Be("Some<Int32>(value = 13)");
    }

    [TestMethod]
    public void ToString_WithNone_ReturnsSomeString()
    {
        // arrange
        var option = Optional.None<int>();

        // act
        var result = option.ToString();

        // assert
        result.Should().Be("None<Int32>");
    }
}

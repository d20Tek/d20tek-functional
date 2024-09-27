namespace D20Tek.Functional.UnitTests;

[TestClass]
public class OptionToFromTests
{
    [TestMethod]
    public void Some_WithValue_ReturnsSome()
    {
        // arrange
        var option = Option.Some<int>(13);

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
        var option = Option.None<string>();

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
        var option = Option.OfNullable<int>(101);

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
        var option = Option.OfNullable<int>(null);

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
        var option = Option.OfObj<string>("test");

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
        var option = Option.OfObj<string>(null);

        // act
        var isSome = option.IsSome;
        var isNone = option.IsNone;

        // assert
        isSome.Should().BeFalse();
        isNone.Should().BeTrue();
    }
}

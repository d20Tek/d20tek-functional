namespace D20Tek.Functional.UnitTests;

[TestClass]
public class ChoiceAsyncTests
{
    [TestMethod]
    public void IsChoice1_WithValueType1_ReturnsTrue()
    {
        // arrange
        var choice = new Choice<int, string>(42);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;

        // assert
        isChoice1.Should().BeTrue();
        isChoice2.Should().BeFalse();
        choice.GetChoice1().Should().Be(42);
        choice.ToString().Should().Be("Choice<Int32>(value = 42)");
    }

    [TestMethod]
    public void IsChoice2_WithValueType2_ReturnsTrue()
    {
        // arrange
        var choice = new Choice<int, string>("foo");

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeTrue();
        choice.GetChoice2().Should().Be("foo");
        choice.ToString().Should().Be("Choice<String>(value = foo)");
    }

    [TestMethod]
    public void Bind_WithValidT1Value_ReturnsChoice1()
    {
        // arrange
        var input = new Choice<string, int>("42");

        // act
        var result = input.Bind(v => TryParse(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsChoice2()
    {
        // arrange
        var input = new Choice<string, int>("non-int-text");

        // act
        var result = input.Bind(v => TryParse(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(0);
    }

    [TestMethod]
    public void Bind_WithT2Value_ReturnsInitialChoice2()
    {
        // arrange
        var input = new Choice<string, int>(101);

        // act
        var result = input.Bind([ExcludeFromCodeCoverage] (v) => TryParse(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(101);
    }

    [TestMethod]
    public void Map_WithChoice1Value_ReturnsChoice1()
    {
        // arrange
        var input = new Choice<int, string>(42);

        // act
        var result = input.Map(v => v * 2);

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(84);
    }

    [TestMethod]
    public void Map_WithChoice2Value_ReturnsChoice1()
    {
        // arrange
        var input = new Choice<int, string>("foo");

        // act
        var result = input.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be("foo");
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Match_WithInvalidType_ThrowsException()
    {
        // arrange
        var input = new Choice<int, string>(null!);

        // act
        var result = input.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
    }

    [TestMethod]
    public void Iter_WithChoice1Value_CallsAction1()
    {
        // arrange
        var output = string.Empty;
        var input = new Choice<int, string>(42);

        // act
        input.Iter(v1 => output = "choice1 called", [ExcludeFromCodeCoverage] (v2) => output = "choice2 called");

        // assert
        output.Should().Be("choice1 called");
    }

    [TestMethod]
    public void Iter_WithChoice2Value_CallsAction2()
    {
        // arrange
        var output = string.Empty;
        var input = new Choice<int, string>("foo");

        // act
        input.Iter([ExcludeFromCodeCoverage] (v1) => output = "choice1 called", v2 => output = "choice2 called");

        // assert
        output.Should().Be("choice2 called");
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Iter_WithInvalidType_ThrowsException()
    {
        // arrange
        var output = string.Empty;
        var input = new Choice<int, string>(null!);

        // act
        input.Iter(
            [ExcludeFromCodeCoverage] (v1) => output = "choice1 called",
            [ExcludeFromCodeCoverage] (v2) => output = "choice2 called");

        // assert
    }

    private static Choice<float, int> TryParse(string text) =>
        float.TryParse(text, out float parsed)
            ? new Choice<float, int>(parsed)
            : new Choice<float, int>(0);
}

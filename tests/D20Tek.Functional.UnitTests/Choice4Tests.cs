﻿namespace D20Tek.Functional.UnitTests;

[TestClass]
public class Choice4Tests
{
    [TestMethod]
    public void IsChoice1_WithValueType1_ReturnsTrue()
    {
        // arrange
        var choice = new Choice<int, string, Guid, decimal>(42);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;

        // assert
        isChoice1.Should().BeTrue();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeFalse();
        isChoice4.Should().BeFalse();
        choice.GetChoice1().Should().Be(42);
        choice.ToString().Should().Be("Choice<Int32>(value = 42)");
    }

    [TestMethod]
    public void IsChoice2_WithValueType2_ReturnsTrue()
    {
        // arrange
        var choice = new Choice<int, string, Guid, decimal>("foo");

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeTrue();
        isChoice3.Should().BeFalse();
        isChoice4.Should().BeFalse();
        choice.GetChoice2().Should().Be("foo");
        choice.ToString().Should().Be("Choice<String>(value = foo)");
    }

    [TestMethod]
    public void IsChoice3_WithValueType3_ReturnsTrue()
    {
        // arrange
        var guid = Guid.NewGuid();
        var choice = new Choice<int, string, Guid, decimal>(guid);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeTrue();
        isChoice4.Should().BeFalse();
        choice.GetChoice3().Should().Be(guid);
        choice.ToString().Should().Be($"Choice<Guid>(value = {guid})");
    }

    [TestMethod]
    public void IsChoice4_WithValueType4_ReturnsTrue()
    {
        // arrange
        var choice = new Choice<int, string, Guid, decimal>(10M);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeFalse();
        isChoice4.Should().BeTrue();
        choice.GetChoice4().Should().Be(10M);
        choice.ToString().Should().Be("Choice<Decimal>(value = 10)");
    }

    [TestMethod]
    public void Bind_WithValidT1Value_ReturnsChoice1()
    {
        // arrange
        var input = new Choice<string, int, Guid, decimal>("42");

        // act
        var result = input.Bind(v => TryParse(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithValidT1ValueAndGuid_ReturnsChoice1()
    {
        // arrange
        var input = new Choice<string, int, Guid, decimal>("42");

        // act
        var result = input.Bind(v => TryParseOrGuid(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithValidT1ValueAndDecimal_ReturnsChoice1()
    {
        // arrange
        var input = new Choice<string, int, Guid, decimal>("42");

        // act
        var result = input.Bind(v => TryParseOrDecimal(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsChoice2()
    {
        // arrange
        var input = new Choice<string, int, Guid, decimal>("non-int-text");

        // act
        var result = input.Bind(v => TryParse(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(0);
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsChoice3()
    {
        // arrange
        var input = new Choice<string, int, Guid, decimal>("non-int-text");

        // act
        var result = input.Bind(v => TryParseOrGuid(v));

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().NotBeEmpty();
    }

    [TestMethod]
    public void Bind_WithInvalidValue_ReturnsChoice4()
    {
        // arrange
        var input = new Choice<string, int, Guid, decimal>("non-int-text");

        // act
        var result = input.Bind(v => TryParseOrDecimal(v));

        // assert
        result.IsChoice4.Should().BeTrue();
        result.GetChoice4().Should().Be(1234M);
    }

    [TestMethod]
    public void Bind_WithT2Value_ReturnsInitialChoice2()
    {
        // arrange
        var input = new Choice<string, int, Guid, decimal>(101);

        // act
        var result = input.Bind([ExcludeFromCodeCoverage] (v) => TryParse(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(101);
    }

    [TestMethod]
    public void Bind_WithT3Value_ReturnsInitialChoice3()
    {
        // arrange
        var guid = Guid.NewGuid();
        var input = new Choice<string, int, Guid, decimal>(guid);

        // act
        var result = input.Bind([ExcludeFromCodeCoverage] (v) => TryParse(v));

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().Be(guid);
    }

    [TestMethod]
    public void Bind_WithT4Value_ReturnsInitialChoice4()
    {
        // arrange
        var input = new Choice<string, int, Guid, decimal>(10M);

        // act
        var result = input.Bind([ExcludeFromCodeCoverage] (v) => TryParse(v));

        // assert
        result.IsChoice4.Should().BeTrue();
        result.GetChoice4().Should().Be(10M);
    }

    [TestMethod]
    public void Map_WithChoice1Value_ReturnsChoice1()
    {
        // arrange
        var input = new Choice<int, string, Guid, decimal>(42);

        // act
        var result = input.Map(v => v * 2);

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(84);
    }

    [TestMethod]
    public void Map_WithChoice2Value_ReturnsChoice2()
    {
        // arrange
        var input = new Choice<int, string, Guid, decimal>("foo");

        // act
        var result = input.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be("foo");
    }

    [TestMethod]
    public void Map_WithChoice3Value_ReturnsChoice3()
    {
        // arrange
        var guid = Guid.NewGuid();
        var input = new Choice<int, string, Guid, decimal>(guid);

        // act
        var result = input.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().Be(guid);
    }

    [TestMethod]
    public void Map_WithChoice4Value_ReturnsChoice4()
    {
        // arrange
        var input = new Choice<int, string, Guid, decimal>(10M);

        // act
        var result = input.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
        result.IsChoice4.Should().BeTrue();
        result.GetChoice4().Should().Be(10M);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Match_WithInvalidType_ThrowsException()
    {
        // arrange
        var input = new Choice<int, string, Guid, decimal>(null!);

        // act
        var result = input.Map([ExcludeFromCodeCoverage] (v) => v * 2);

        // assert
    }

    [TestMethod]
    public void Iter_WithChoice1Value_CallsAction1()
    {
        // arrange
        var output = string.Empty;
        var input = new Choice<int, string, Guid, decimal>(42);

        // act
        input.Iter(
            v1 => output = "choice1 called",
            [ExcludeFromCodeCoverage] (v2) => output = "choice2 called",
            [ExcludeFromCodeCoverage] (v3) => output = "choice3 called",
            [ExcludeFromCodeCoverage] (v4) => output = "choice4 called");

        // assert
        output.Should().Be("choice1 called");
    }

    [TestMethod]
    public void Iter_WithChoice2Value_CallsAction2()
    {
        // arrange
        var output = string.Empty;
        var input = new Choice<int, string, Guid, decimal>("foo");

        // act
        input.Iter(
            [ExcludeFromCodeCoverage] (v1) => output = "choice1 called",
            v2 => output = "choice2 called",
            [ExcludeFromCodeCoverage] (v3) => output = "choice3 called",
            [ExcludeFromCodeCoverage] (v4) => output = "choice4 called");

        // assert
        output.Should().Be("choice2 called");
    }

    [TestMethod]
    public void Iter_WithChoice3Value_CallsAction3()
    {
        // arrange
        var output = string.Empty;
        var input = new Choice<int, string, Guid, decimal>(Guid.NewGuid());

        // act
        input.Iter(
            [ExcludeFromCodeCoverage] (v1) => output = "choice1 called",
            [ExcludeFromCodeCoverage] (v2) => output = "choice2 called",
            v3 => output = "choice3 called",
            [ExcludeFromCodeCoverage] (v4) => output = "choice4 called");

        // assert
        output.Should().Be("choice3 called");
    }

    [TestMethod]
    public void Iter_WithChoice4Value_CallsAction4()
    {
        // arrange
        var output = string.Empty;
        var input = new Choice<int, string, Guid, decimal>(10M);

        // act
        input.Iter(
            [ExcludeFromCodeCoverage] (v1) => output = "choice1 called",
            [ExcludeFromCodeCoverage] (v2) => output = "choice2 called",
            [ExcludeFromCodeCoverage] (v3) => output = "choice3 called",
            (v4) => output = "choice4 called");

        // assert
        output.Should().Be("choice4 called");
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Iter_WithInvalidType_ThrowsException()
    {
        // arrange
        var output = string.Empty;
        var input = new Choice<int, string, Guid, decimal>(null!);

        // act
        input.Iter(
            [ExcludeFromCodeCoverage] (v1) => output = "choice1 called",
            [ExcludeFromCodeCoverage] (v2) => output = "choice2 called",
            [ExcludeFromCodeCoverage] (v3) => output = "choice3 called",
            [ExcludeFromCodeCoverage] (v4) => output = "choice4 called");

        // assert
    }

    private static Choice<float, int, Guid, decimal> TryParse(string text) =>
        float.TryParse(text, out float parsed)
            ? new Choice<float, int, Guid, decimal>(parsed)
            : new Choice<float, int, Guid, decimal>(0);

    private static Choice<float, int, Guid, decimal> TryParseOrGuid(string text) =>
        float.TryParse(text, out float parsed)
            ? new Choice<float, int, Guid, decimal>(parsed)
            : new Choice<float, int, Guid, decimal>(Guid.NewGuid());

    private static Choice<float, int, Guid, decimal> TryParseOrDecimal(string text) =>
        float.TryParse(text, out float parsed)
            ? new Choice<float, int, Guid, decimal>(parsed)
            : new Choice<float, int, Guid, decimal>(1234M);
}

using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class Choice5AsyncTests
{
    [TestMethod]
    public void IsChoice1_WithValueType1_ReturnsTrue()
    {
        // arrange
        var choice = new ChoiceAsync<int, string, Guid, decimal, DateTime>(42);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;
        var isChoice5 = choice.IsChoice5;

        // assert
        isChoice1.Should().BeTrue();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeFalse();
        isChoice4.Should().BeFalse();
        isChoice5.Should().BeFalse();
        choice.GetChoice1().Should().Be(42);
        choice.ToString().Should().Be("ChoiceAsync<Int32>(value = 42)");
    }

    [TestMethod]
    public void IsChoice2_WithValueType2_ReturnsTrue()
    {
        // arrange
        var choice = new ChoiceAsync<int, string, Guid, decimal, DateTime>("foo");

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;
        var isChoice5 = choice.IsChoice5;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeTrue();
        isChoice3.Should().BeFalse();
        isChoice4.Should().BeFalse();
        isChoice5.Should().BeFalse();
        choice.GetChoice2().Should().Be("foo");
        choice.ToString().Should().Be("ChoiceAsync<String>(value = foo)");
    }

    [TestMethod]
    public void IsChoice3_WithValueType3_ReturnsTrue()
    {
        // arrange
        var guid = Guid.NewGuid();
        var choice = new ChoiceAsync<int, string, Guid, decimal, DateTime>(guid);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;
        var isChoice5 = choice.IsChoice5;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeTrue();
        isChoice4.Should().BeFalse();
        isChoice5.Should().BeFalse();
        choice.GetChoice3().Should().Be(guid);
        choice.ToString().Should().Be($"ChoiceAsync<Guid>(value = {guid})");
    }

    [TestMethod]
    public void IsChoice4_WithValueType4_ReturnsTrue()
    {
        // arrange
        var choice = new ChoiceAsync<int, string, Guid, decimal, DateTime>(10M);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;
        var isChoice5 = choice.IsChoice5;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeFalse();
        isChoice4.Should().BeTrue();
        isChoice5.Should().BeFalse();
        choice.GetChoice4().Should().Be(10M);
        choice.ToString().Should().Be("ChoiceAsync<Decimal>(value = 10)");
    }

    [TestMethod]
    public void IsChoice5_WithValueType5_ReturnsTrue()
    {
        // arrange
        var choice = new ChoiceAsync<int, string, Guid, decimal, bool>(true);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;
        var isChoice4 = choice.IsChoice4;
        var isChoice5 = choice.IsChoice5;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeFalse();
        isChoice4.Should().BeFalse();
        isChoice5.Should().BeTrue();
        choice.GetChoice5().Should().Be(true);
        choice.ToString().Should().Be("ChoiceAsync<Boolean>(value = True)");
    }

    [TestMethod]
    public async Task BindAsync_WithValidT1Value_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>("42");

        // act
        var result = await input.BindAsync(v => TryParseAsync(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithValidT1ValueAndGuid_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>("42");

        // act
        var result = await input.BindAsync(v => TryParseOrGuidAsync(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithValidT1ValueAndDate_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>("42");

        // act
        var result = await input.BindAsync(v => TryParseAsync(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithValidT1ValueAndDecimal_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>("42");

        // act
        var result = await input.BindAsync(v => TryParseOrDecimalAsync(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsChoice2()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>("non-int-text");

        // act
        var result = await input.BindAsync(v => TryParseAsync(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(0);
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsChoice3()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>("non-int-text");

        // act
        var result = await input.BindAsync(v => TryParseOrGuidAsync(v));

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsChoice4()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>("non-int-text");

        // act
        var result = await input.BindAsync(v => TryParseOrDecimalAsync(v));

        // assert
        result.IsChoice4.Should().BeTrue();
        result.GetChoice4().Should().Be(1234M);
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsChoice5()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>("non-int-text");

        // act
        var result = await input.BindAsync(v => TryParseOrDateAsync(v));

        // assert
        result.IsChoice5.Should().BeTrue();
        result.GetChoice5().Should().Be(new DateTime(2024, 9, 30));
    }

    [TestMethod]
    public async Task BindAsync_WithT2Value_ReturnsInitialChoice2()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>(101);

        // act
        var result = await input.BindAsync([ExcludeFromCodeCoverage] (v) => TryParseAsync(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(101);
    }

    [TestMethod]
    public async Task BindAsync_WithT3Value_ReturnsInitialChoice3()
    {
        // arrange
        var guid = Guid.NewGuid();
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>(guid);

        // act
        var result = await input.BindAsync([ExcludeFromCodeCoverage] (v) => TryParseAsync(v));

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().Be(guid);
    }

    [TestMethod]
    public async Task BindAsync_WithT4Value_ReturnsInitialChoice4()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>(10M);

        // act
        var result = await input.BindAsync([ExcludeFromCodeCoverage] (v) => TryParseAsync(v));

        // assert
        result.IsChoice4.Should().BeTrue();
        result.GetChoice4().Should().Be(10M);
    }

    [TestMethod]
    public async Task BindAsync_WithT5Value_ReturnsInitialChoice5()
    {
        // arrange
        var date = DateTime.Today;
        var input = new ChoiceAsync<string, int, Guid, decimal, DateTime>(date);

        // act
        var result = await input.BindAsync([ExcludeFromCodeCoverage] (v) => TryParseAsync(v));

        // assert
        result.IsChoice5.Should().BeTrue();
        result.GetChoice5().Should().Be(date);
    }

    [TestMethod]
    public async Task MapAsync_WithChoice1Value_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(42);

        // act
        var result = await input.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(84);
    }

    [TestMethod]
    public async Task MapAsync_WithChoice2Value_ReturnsChoice2()
    {
        // arrange
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>("foo");

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be("foo");
    }

    [TestMethod]
    public async Task MapAsync_WithChoice3Value_ReturnsChoice3()
    {
        // arrange
        var guid = Guid.NewGuid();
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(guid);

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().Be(guid);
    }

    [TestMethod]
    public async Task MapAsync_WithChoice4Value_ReturnsChoice4()
    {
        // arrange
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(10M);

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsChoice4.Should().BeTrue();
        result.GetChoice4().Should().Be(10M);
    }

    [TestMethod]
    public async Task MapAsync_WithChoice5Value_ReturnsChoice5()
    {
        // arrange
        var date = DateTime.Today;
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(date);

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsChoice5.Should().BeTrue();
        result.GetChoice5().Should().Be(date);
    }

    [TestMethod]
    public async Task MatchAsync_WithInvalidType_ThrowsException()
    {
        // arrange
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(null!);

        // act
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>([ExcludeFromCodeCoverage] () =>
            input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2)));
    }

    [TestMethod]
    public async Task IterAsync_WithChoice1Value_CallsAction1()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(42);

        // act
        await input.IterAsync(
            v1 => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v3) => { output = "choice3 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v4) => { output = "choice4 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v5) => { output = "choice5 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice1 called");
    }

    [TestMethod]
    public async Task IterAsync_WithChoice2Value_CallsAction2()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>("foo");

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            v2 => { output = "choice2 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v3) => { output = "choice3 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v4) => { output = "choice4 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v5) => { output = "choice5 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice2 called");
    }

    [TestMethod]
    public async Task IterAsync_WithChoice3Value_CallsAction3()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(Guid.NewGuid());

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; },
            v3 => { output = "choice3 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v4) => { output = "choice4 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v5) => { output = "choice5 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice3 called");
    }

    [TestMethod]
    public async Task IterAsync_WithChoice4Value_CallsAction4()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(10M);

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v3) => { output = "choice3 called"; return Task.CompletedTask; },
            (v4) => { output = "choice4 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v5) => { output = "choice5 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice4 called");
    }

    [TestMethod]
    public async Task IterAsync_WithChoice5Value_CallsAction5()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(DateTime.Now);

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v3) => { output = "choice3 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v4) => { output = "choice4 called"; return Task.CompletedTask; },
            v5 => { output = "choice5 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice5 called");
    }

    [TestMethod]
    public async Task IterAsync_WithInvalidType_ThrowsException()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid, decimal, DateTime>(null!);

        // act
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>([ExcludeFromCodeCoverage] () => input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v3) => { output = "choice3 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v4) => { output = "choice4 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v5) => { output = "choice5 called"; return Task.CompletedTask; }));
    }

    private static Task<ChoiceAsync<float, int, Guid, decimal, DateTime>> TryParseAsync(string text) =>
        Task.FromResult(float.TryParse(text, out float parsed)
                             ? new ChoiceAsync<float, int, Guid, decimal, DateTime>(parsed)
                             : new ChoiceAsync<float, int, Guid, decimal, DateTime>(0));

    private static Task<ChoiceAsync<float, int, Guid, decimal, DateTime>> TryParseOrGuidAsync(string text) =>
        Task.FromResult(float.TryParse(text, out float parsed)
                             ? new ChoiceAsync<float, int, Guid, decimal, DateTime>(parsed)
                             : new ChoiceAsync<float, int, Guid, decimal, DateTime>(Guid.NewGuid()));

    private static Task<ChoiceAsync<float, int, Guid, decimal, DateTime>> TryParseOrDecimalAsync(string text) =>
        Task.FromResult(float.TryParse(text, out float parsed)
                             ? new ChoiceAsync<float, int, Guid, decimal, DateTime>(parsed)
                             : new ChoiceAsync<float, int, Guid, decimal, DateTime>(1234M));

    [ExcludeFromCodeCoverage]
    private static Task<ChoiceAsync<float, int, Guid, decimal, DateTime>> TryParseOrDateAsync(string text) =>
        Task.FromResult(float.TryParse(text, out float parsed)
                             ? new ChoiceAsync<float, int, Guid, decimal, DateTime>(parsed)
                             : new ChoiceAsync<float, int, Guid, decimal, DateTime>(new DateTime(2024, 9, 30)));
}

using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class Choice3AsyncTests
{
    [TestMethod]
    public void IsChoice1_WithValueType1_ReturnsTrue()
    {
        // arrange
        var choice = new ChoiceAsync<int, string, Guid>(42);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;

        // assert
        isChoice1.Should().BeTrue();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeFalse();
        choice.GetChoice1().Should().Be(42);
        choice.ToString().Should().Be("ChoiceAsync<Int32>(value = 42)");
    }

    [TestMethod]
    public void IsChoice2_WithValueType2_ReturnsTrue()
    {
        // arrange
        var choice = new ChoiceAsync<int, string, Guid>("foo");

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeTrue();
        isChoice3.Should().BeFalse();
        choice.GetChoice2().Should().Be("foo");
        choice.ToString().Should().Be("ChoiceAsync<String>(value = foo)");
    }

    [TestMethod]
    public void IsChoice3_WithValueType3_ReturnsTrue()
    {
        // arrange
        var guid = Guid.NewGuid();
        var choice = new ChoiceAsync<int, string, Guid>(guid);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;
        var isChoice3 = choice.IsChoice3;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeFalse();
        isChoice3.Should().BeTrue();
        choice.GetChoice3().Should().Be(guid);
        choice.ToString().Should().Be($"ChoiceAsync<Guid>(value = {guid})");
    }

    [TestMethod]
    public async Task BindAsync_WithValidT1Value_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid>("42");

        // act
        var result = await input.BindAsync(async (v) => await TryParseAsync(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithValidT1ValueAndGuid_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid>("42");

        // act
        var result = await input.BindAsync(async (v) => await TryParseOrGuidAsync(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsChoice2()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid>("non-int-text");

        // act
        var result = await input.BindAsync(async(v) => await TryParseAsync(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(0);
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsChoice3()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid>("non-int-text");

        // act
        var result = await input.BindAsync(async (v) => await TryParseOrGuidAsync(v));

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task BindAsync_WithT2Value_ReturnsInitialChoice2()
    {
        // arrange
        var input = new ChoiceAsync<string, int, Guid>(101);

        // act
        var result = await input.BindAsync(TryParseAsync);

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(101);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public async Task BindAsync_WithT3Value_ReturnsInitialChoice3()
    {
        // arrange
        var guid = Guid.NewGuid();
        var input = new ChoiceAsync<string, int, Guid>(guid);

        // act
        var result = await input.BindAsync(TryParseAsync);

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().Be(guid);
    }

    [TestMethod]
    public async Task MapAsync_WithChoice1Value_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<int, string, Guid>(42);

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
        var input = new ChoiceAsync<int, string, Guid>("foo");

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
        var input = new ChoiceAsync<int, string, Guid>(guid);

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsChoice3.Should().BeTrue();
        result.GetChoice3().Should().Be(guid);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public async Task MapAsync_WithInvalidType_ThrowsException()
    {
        // arrange
        var input = new ChoiceAsync<int, string, Guid>(null!);

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
    }

    [TestMethod]
    public async Task IterAsync_WithChoice1Value_CallsAction1()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid>(42);

        // act
        await input.IterAsync(
            v1 => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v3) => { output = "choice3 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice1 called");
    }

    [TestMethod]
    public async Task IterAsync_WithChoice2Value_CallsAction2()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid>("foo");

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            v2 => { output = "choice2 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v3) => { output = "choice3 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice2 called");
    }

    [TestMethod]
    public async Task IterAsync_WithChoice3Value_CallsAction3()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid>(Guid.NewGuid());

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; },
            v3 => { output = "choice3 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice3 called");
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public async Task IterAsync_WithInvalidType_ThrowsException()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string, Guid>(null!);

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v3) => { output = "choice3 called"; return Task.CompletedTask; });

        // assert
    }

    private static Task<ChoiceAsync<float, int, Guid>> TryParseAsync(string text) =>
        Task.FromResult(float.TryParse(text, out float parsed)
                             ? new ChoiceAsync<float, int, Guid>(parsed)
                             : new ChoiceAsync<float, int, Guid>(0));

    private static Task<ChoiceAsync<float, int, Guid>> TryParseOrGuidAsync(string text) =>
        Task.FromResult(float.TryParse(text, out float parsed)
                            ? new ChoiceAsync<float, int, Guid>(parsed)
                            : new ChoiceAsync<float, int, Guid>(Guid.NewGuid()));
}

using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class ChoiceAsyncTests
{
    [TestMethod]
    public void IsChoice1_WithValueType1_ReturnsTrue()
    {
        // arrange
        var choice = new ChoiceAsync<int, string>(42);

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;

        // assert
        isChoice1.Should().BeTrue();
        isChoice2.Should().BeFalse();
        choice.GetChoice1().Should().Be(42);
        choice.ToString().Should().Be("ChoiceAsync<Int32>(value = 42)");
    }

    [TestMethod]
    public void IsChoice2_WithValueType2_ReturnsTrue()
    {
        // arrange
        var choice = new ChoiceAsync<int, string>("foo");

        // act
        var isChoice1 = choice.IsChoice1;
        var isChoice2 = choice.IsChoice2;

        // assert
        isChoice1.Should().BeFalse();
        isChoice2.Should().BeTrue();
        choice.GetChoice2().Should().Be("foo");
        choice.ToString().Should().Be("ChoiceAsync<String>(value = foo)");
    }

    [TestMethod]
    public async Task Bind_WithValidT1Value_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<string, int>("42");

        // act
        var result = await input.BindAsync(v => TryParseAsync(v));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(42);
    }

    [TestMethod]
    public async Task Bind_WithInvalidValue_ReturnsChoice2()
    {
        // arrange
        var input = new ChoiceAsync<string, int>("non-int-text");

        // act
        var result = await input.BindAsync(v => TryParseAsync(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(0);
    }

    [TestMethod]
    public async Task Bind_WithT2Value_ReturnsInitialChoice2()
    {
        // arrange
        var input = new ChoiceAsync<string, int>(101);

        // act
        var result = await input.BindAsync([ExcludeFromCodeCoverage] (v) => TryParseAsync(v));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be(101);
    }

    [TestMethod]
    public async Task Map_WithChoice1Value_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<int, string>(42);

        // act
        var result = await input.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.IsChoice1.Should().BeTrue();
        result.GetChoice1().Should().Be(84);
    }

    [TestMethod]
    public async Task Map_WithChoice2Value_ReturnsChoice1()
    {
        // arrange
        var input = new ChoiceAsync<int, string>("foo");

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsChoice2.Should().BeTrue();
        result.GetChoice2().Should().Be("foo");
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public async Task Match_WithInvalidType_ThrowsException()
    {
        // arrange
        var input = new ChoiceAsync<int, string>(null!);

        // act
        var result = await input.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
    }

    [TestMethod]
    public async Task Iter_WithChoice1Value_CallsAction1()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string>(42);

        // act
        await input.IterAsync(
            v1 => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice1 called");
    }

    [TestMethod]
    public async Task Iter_WithChoice2Value_CallsAction2()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string>("foo");

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            v2 => { output = "choice2 called"; return Task.CompletedTask; });

        // assert
        output.Should().Be("choice2 called");
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public async Task Iter_WithInvalidType_ThrowsException()
    {
        // arrange
        var output = string.Empty;
        var input = new ChoiceAsync<int, string>(null!);

        // act
        await input.IterAsync(
            [ExcludeFromCodeCoverage] (v1) => { output = "choice1 called"; return Task.CompletedTask; },
            [ExcludeFromCodeCoverage] (v2) => { output = "choice2 called"; return Task.CompletedTask; });

        // assert
    }

    private static Task<Choice<float, int>> TryParseAsync(string text) =>
        Task.FromResult(float.TryParse(text, out float parsed)
                             ? new Choice<float, int>(parsed)
                             : new Choice<float, int>(0));
}

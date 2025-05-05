using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class OptionBindMapAsyncTests
{
    [TestMethod]
    public async Task BindAsync_WithValidValue_ReturnsSome()
    {
        // arrange
        var option = Option<string>.Some("42");

        // act
        var result = await option.BindAsync(v => OptionHelper.TryParseAsync(v));

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public async Task TaskBindAsync_WithValidValue_ReturnsSome()
    {
        // arrange
        var option = Task.FromResult(Option<string>.Some("42"));

        // act
        var result = await option.BindAsync(v => OptionHelper.TryParseAsync(v));

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public async Task BindAsync_WithInvalidValue_ReturnsNone()
    {
        // arrange
        var option = Option<string>.Some("non-int-text");

        // act
        var result = await option.BindAsync(v => OptionHelper.TryParseAsync(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskBindAsync_WithInvalidValue_ReturnsNone()
    {
        // arrange
        var option = Option<string>.Some("non-int-text");

        // act
        var result = await option.BindAsync(v => OptionHelper.TryParseAsync(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task BindAsync_WithNoneOption_ReturnsNone()
    {
        // arrange
        var option = Option<string>.None();

        // act
        var result = await option.BindAsync([ExcludeFromCodeCoverage] (v) => OptionHelper.TryParseAsync(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskBindAsync_WithNoneOption_ReturnsNone()
    {
        // arrange
        var option = Task.FromResult(Option<string>.None());

        // act
        var result = await option.BindAsync([ExcludeFromCodeCoverage] (v) => OptionHelper.TryParseAsync(v));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task MapAsync_WithValidValue_ReturnsSome()
    {
        // arrange
        var option = Option<int>.Some(42);

        // act
        var result = await option.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(84);
    }

    [TestMethod]
    public async Task TaskMapAsync_WithValidValue_ReturnsSome()
    {
        // arrange
        var option = Task.FromResult(Option<int>.Some(42));

        // act
        var result = await option.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.IsSome.Should().BeTrue();
        result.Get().Should().Be(84);
    }

    [TestMethod]
    public async Task MapAsync_WithNoneOption_ReturnsSome()
    {
        // arrange
        var option = Option<int>.None();

        // act
        var result = await option.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsNone.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskMapAsync_WithNoneOption_ReturnsSome()
    {
        // arrange
        var option = Task.FromResult(Option<int>.None());

        // act
        var result = await option.MapAsync([ExcludeFromCodeCoverage] (v) => Task.FromResult(v * 2));

        // assert
        result.IsNone.Should().BeTrue();
    }
}

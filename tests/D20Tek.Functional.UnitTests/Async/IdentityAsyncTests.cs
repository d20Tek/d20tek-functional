using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class IdentityAsyncTests
{
    [TestMethod]
    public async Task BindAsync_WithValue_ReturnsIdentity()
    {
        // arrange
        var id = Identity<string>.Create("42");

        // act
        var result = await id.BindAsync(v => TryParseAsync(v));

        // assert
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public async Task TaskBindAsync_WithValue_ReturnsIdentity()
    {
        // arrange
        var id = Task.FromResult(Identity<string>.Create("42"));

        // act
        var result = await id.BindAsync(v => TryParseAsync(v));

        // assert
        result.Get().Should().Be(42);
    }

    [TestMethod]
    public async Task ExistsAsync_WithPositivePredicate_ReturnsTrue()
    {
        // arrange
        var item = Identity<int>.Create(42);

        // act
        var result = await item.ExistsAsync(x => Task.FromResult(x > 10));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task TaskExistsAsync_WithPositivePredicate_ReturnsTrue()
    {
        // arrange
        var item = Task.FromResult(Identity<int>.Create(42));

        // act
        var result = await item.ExistsAsync(x => Task.FromResult(x > 10));

        // assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task ExistsAsync_WithNegativePredicate_ReturnsFalse()
    {
        // arrange
        var item = Identity<int>.Create(42);

        // act
        var result = await item.ExistsAsync(x => Task.FromResult(x == 10));

        // assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task IterAsync_WithValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var option = Identity<string>.Create("Hello World");

        // act
        await option.IterAsync(x => { output = x; return Task.CompletedTask; });

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public async Task TaskIterAsync_WithValue_PerformsAction()
    {
        // arrange
        var output = string.Empty;
        var option = Task.FromResult(Identity<string>.Create("Hello World"));

        // act
        await option.IterAsync(x => { output = x; return Task.CompletedTask; });

        // assert
        output.Should().Be("Hello World");
    }

    [TestMethod]
    public async Task MapAsync_WithValue_ReturnsNewIdentity()
    {
        // arrange
        var option = Identity<int>.Create(42);

        // act
        var result = await option.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.Get().Should().Be(84);
    }

    [TestMethod]
    public async Task TaskMapAsync_WithValue_ReturnsNewIdentity()
    {
        // arrange
        var option = Task.FromResult(Identity<int>.Create(42));

        // act
        var result = await option.MapAsync(v => Task.FromResult(v * 2));

        // assert
        result.Get().Should().Be(84);
    }

    [ExcludeFromCodeCoverage]
    private static Task<Identity<int>> TryParseAsync(string text) =>
        Task.FromResult((int.TryParse(text, out int parsed) ? parsed : 0).ToIdentity());
}

using D20Tek.Functional.Async;

namespace D20Tek.Functional.UnitTests.Async;

[TestClass]
public class StateAsyncTests
{
    public sealed record TestState(string Name, int Value, bool IsActive = true) : IState;

    [TestMethod]
    public async Task BindAsync_UpdatesCurrentState_ReturnNewState()
    {
        // arrange
        var prevState = new TestState("prev", 5);

        // act
        var newState = await prevState.BindAsync(s => Task.FromResult(s with { Name = "new", Value = 100, IsActive = false }));

        // assert
        newState.Should().NotBe(prevState);
        newState.Name.Should().Be("new");
        newState.Value.Should().Be(100);
        newState.IsActive.Should().BeFalse();
    }

    [TestMethod]
    public async Task IterAsync_CallsAction_ReturnsSameState()
    {
        // arrange
        var state = new TestState("prev", 5);
        var output = string.Empty;

        // act
        var result = await state.IterAsync(s => { output = $"Input state: {s.Name}"; return Task.CompletedTask; });

        // assert
        result.Should().Be(state);
        output.Should().Be("Input state: prev");
    }

    [TestMethod]
    public async Task MapAsync_UpdatesCurrentState_ReturnNewState()
    {
        // arrange
        var prevState = new TestState("prev", 5);

        // act
        var newState = await prevState.MapAsync(s => Task.FromResult(s with { Name = "new", Value = 100, IsActive = false }));

        // assert
        newState.Should().NotBe(prevState);
        newState.Name.Should().Be("new");
        newState.Value.Should().Be(100);
        newState.IsActive.Should().BeFalse();
    }

    [TestMethod]
    public async Task TaskBindAsync_UpdatesCurrentState_ReturnNewState()
    {
        // arrange
        var prevState = Task.FromResult(new TestState("prev", 5));

        // act
        var newState = await prevState.BindAsync(s => Task.FromResult(s with { Name = "new", Value = 100, IsActive = false }));

        // assert
        newState.Should().NotBe(prevState);
        newState.Name.Should().Be("new");
        newState.Value.Should().Be(100);
        newState.IsActive.Should().BeFalse();
    }

    [TestMethod]
    public async Task TaskIterAsync_CallsAction_ReturnsSameState()
    {
        // arrange
        var state = Task.FromResult(new TestState("prev", 5));
        var output = string.Empty;

        // act
        var result = await state.IterAsync(s => { output = $"Input state: {s.Name}"; return Task.CompletedTask; });

        // assert
        result.Should().Be(state.Result);
        output.Should().Be("Input state: prev");
    }

    [TestMethod]
    public async Task TaskMapAsync_UpdatesCurrentState_ReturnNewState()
    {
        // arrange
        var prevState = Task.FromResult(new TestState("prev", 5));

        // act
        var newState = await prevState.MapAsync(s => Task.FromResult(s with { Name = "new", Value = 100, IsActive = false }));

        // assert
        newState.Should().NotBe(prevState);
        newState.Name.Should().Be("new");
        newState.Value.Should().Be(100);
        newState.IsActive.Should().BeFalse();
    }
}

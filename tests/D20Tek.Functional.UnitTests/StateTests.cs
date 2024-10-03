namespace D20Tek.Functional.UnitTests;

[TestClass]
public class StateTests
{
    public sealed record TestState(string Name, int Value, bool IsActive = true) : IState;

    [TestMethod]
    public void Bind_UpdatesCurrentState_ReturnNewState()
    {
        // arrange
        var prevState = new TestState("prev", 5);

        // act
        var newState = prevState.Bind(s => s with { Name = "new", Value = 100, IsActive = false });

        // assert
        newState.Should().NotBe(prevState);
        newState.Name.Should().Be("new");
        newState.Value.Should().Be(100);
        newState.IsActive.Should().BeFalse();
    }

    [TestMethod]
    public void Iter_CallsAction_ReturnsSameState()
    {
        // arrange
        var state = new TestState("prev", 5);
        var output = string.Empty;

        // act
        var result = state.Iter(s => output = $"Input state: {s.Name}");

        // assert
        result.Should().Be(state);
        output.Should().Be("Input state: prev");
    }

    [TestMethod]
    public void Map_UpdatesCurrentState_ReturnNewState()
    {
        // arrange
        var prevState = new TestState("prev", 5);

        // act
        var newState = prevState.Map(s => s with { Name = "new", Value = 100, IsActive = false });

        // assert
        newState.Should().NotBe(prevState);
        newState.Name.Should().Be("new");
        newState.Value.Should().Be(100);
        newState.IsActive.Should().BeFalse();
    }
}

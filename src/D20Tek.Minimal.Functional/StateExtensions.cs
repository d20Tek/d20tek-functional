namespace D20Tek.Minimal.Functional;

public static class StateExtensions
{
    public static TState UpdateState<TState>(this TState currentState, Func<TState, TState> func)
        where TState : State =>
        func(currentState);
}

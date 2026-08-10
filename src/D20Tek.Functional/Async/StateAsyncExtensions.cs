namespace D20Tek.Functional.Async;

/// <summary>
/// Async extension methods providing monadic operations (Bind, Map, Iter) for types implementing <see cref="IState"/>.
/// </summary>
public static class StateAsyncExtensions
{
    /// <summary>
    /// Asynchronous monadic bind: transforms the current state into a new state type.
    /// </summary>
    public static async Task<TOut> BindAsync<TIn, TOut>(this TIn state, Func<TIn, Task<TOut>> bind)
        where TIn : IState
        where TOut : IState =>
        await bind(state);

    /// <summary>
    /// Awaits a state task, then performs an asynchronous bind.
    /// </summary>
    public static async Task<TOut> BindAsync<TIn, TOut>(this Task<TIn> state, Func<TIn, Task<TOut>> bind)
        where TIn : IState
        where TOut : IState =>
        await bind(await state);

    /// <summary>
    /// Executes an async side-effect on the state and returns it unchanged.
    /// </summary>
    public static async Task<T> IterAsync<T>(this T state, Func<T, Task> action)
        where T : IState
    {
        await action(state);
        return state;
    }

    /// <summary>
    /// Awaits a state task, then executes an async side-effect and returns the state.
    /// </summary>
    public static async Task<T> IterAsync<T>(this Task<T> state, Func<T, Task> action)
        where T : IState =>
        await (await state).IterAsync(action);

    /// <summary>
    /// Asynchronously maps the state to a non-state value.
    /// </summary>
    public static async Task<TOut> MapAsync<TIn, TOut>(this TIn state, Func<TIn, Task<TOut>> mapper)
        where TIn : IState
        where TOut : notnull =>
        await mapper(state);

    /// <summary>
    /// Awaits a state task, then asynchronously maps to a non-state value.
    /// </summary>
    public static async Task<TOut> MapAsync<TIn, TOut>(this Task<TIn> state, Func<TIn, Task<TOut>> mapper)
        where TIn : IState
        where TOut : notnull =>
        await mapper(await state);
}

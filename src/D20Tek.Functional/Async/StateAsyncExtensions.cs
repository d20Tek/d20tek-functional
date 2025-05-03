namespace D20Tek.Functional.Async;

public static class StateAsyncExtensions
{
    public static async Task<TOut> BindAsync<TIn, TOut>(this TIn state, Func<TIn, Task<TOut>> bind)
        where TIn : IState
        where TOut : IState =>
        await bind(state);

    public static async Task<TOut> BindAsync<TIn, TOut>(this Task<TIn> state, Func<TIn, Task<TOut>> bind)
        where TIn : IState
        where TOut : IState =>
        await bind(await state);

    public static async Task<T> IterAsync<T>(this T state, Func<T, Task> action)
        where T : IState
    {
        await action(state);
        return state;
    }

    public static async Task<T> IterAsync<T>(this Task<T> state, Func<T, Task> action)
        where T : IState =>
        await (await state).IterAsync(action);

    public static async Task<TOut> MapAsync<TIn, TOut>(this TIn state, Func<TIn, Task<TOut>> mapper)
        where TIn : IState
        where TOut : notnull =>
        await mapper(state);

    public static async Task<TOut> MapAsync<TIn, TOut>(this Task<TIn> state, Func<TIn, Task<TOut>> mapper)
        where TIn : IState
        where TOut : notnull =>
        await mapper(await state);
}

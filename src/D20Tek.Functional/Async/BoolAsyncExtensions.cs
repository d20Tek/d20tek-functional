namespace D20Tek.Functional.Async;

/// <summary>
/// Async extension methods for <see cref="bool"/> enabling functional branching with asynchronous handlers.
/// </summary>
public static class BoolAsyncExtensions
{
    /// <summary>
    /// Asynchronously evaluates one of two functions based on the boolean value and returns the result.
    /// </summary>
    /// <typeparam name="TOut">The return type of both branches.</typeparam>
    /// <param name="condition">The boolean condition to evaluate.</param>
    /// <param name="thenFunc">Async function to invoke when <c>true</c>.</param>
    /// <param name="elseFunc">Async function to invoke when <c>false</c>.</param>
    public static async Task<TOut> IfTrueOrElseAsync<TOut>(
        this bool condition, Func<Task<TOut>> thenFunc, Func<Task<TOut>> elseFunc) =>
        condition ? await thenFunc() : await elseFunc();

    /// <summary>
    /// Asynchronously executes one of two actions based on the boolean value.
    /// </summary>
    /// <param name="condition">The boolean condition to evaluate.</param>
    /// <param name="thenAction">Async action to execute when <c>true</c>.</param>
    /// <param name="elseAction">Optional async action to execute when <c>false</c>.</param>
    public static async Task IfTrueOrElseAsync(
        this bool condition, Func<Task> thenAction, Func<Task>? elseAction = null)
    {
        if (condition)
            await thenAction();
        else if (elseAction is not null)
            await elseAction.Invoke();
    }

    /// <summary>
    /// Awaits the boolean task, then asynchronously evaluates one of two functions.
    /// </summary>
    /// <typeparam name="TOut">The return type of both branches.</typeparam>
    /// <param name="condition">A task producing the boolean condition.</param>
    /// <param name="thenFunc">Async function to invoke when <c>true</c>.</param>
    /// <param name="elseFunc">Async function to invoke when <c>false</c>.</param>
    public static async Task<TOut> IfTrueOrElseAsync<TOut>(
        this Task<bool> condition, Func<Task<TOut>> thenFunc, Func<Task<TOut>> elseFunc) =>
        await(await condition).IfTrueOrElseAsync(thenFunc, elseFunc);

    /// <summary>
    /// Awaits the boolean task, then asynchronously executes one of two actions.
    /// </summary>
    /// <param name="condition">A task producing the boolean condition.</param>
    /// <param name="thenAction">Async action to execute when <c>true</c>.</param>
    /// <param name="elseAction">Optional async action to execute when <c>false</c>.</param>
    public static async Task IfTrueOrElseAsync(
        this Task<bool> condition, Func<Task> thenAction, Func<Task>? elseAction = null) =>
        await (await condition).IfTrueOrElseAsync(thenAction, elseAction);
}

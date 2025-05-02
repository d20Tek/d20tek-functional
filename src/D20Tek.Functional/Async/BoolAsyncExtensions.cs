namespace D20Tek.Functional.Async;

public static class BoolAsyncExtensions
{
    public static async Task<TOut> IfTrueOrElseAsync<TOut>(
        this bool condition,
        Func<Task<TOut>> thenFunc,
        Func<Task<TOut>> elseFunc) =>
        condition ? await thenFunc() : await elseFunc();

    public static async Task IfTrueOrElseAsync(
        this bool condition,
        Func<Task> thenAction,
        Func<Task>? elseAction = null)
    {
        if (condition)
            await thenAction();
        else if (elseAction is not null)
            await elseAction.Invoke();
    }

    public static async Task<TOut> IfTrueOrElseAsync<TOut>(
        this Task<bool> condition,
        Func<Task<TOut>> thenFunc,
        Func<Task<TOut>> elseFunc) =>
        await(await condition).IfTrueOrElseAsync(thenFunc, elseFunc);

    public static async Task IfTrueOrElseAsync(
    this Task<bool> condition,
    Func<Task> thenAction,
    Func<Task>? elseAction = null) =>
        await (await condition).IfTrueOrElseAsync(thenAction, elseAction);
}

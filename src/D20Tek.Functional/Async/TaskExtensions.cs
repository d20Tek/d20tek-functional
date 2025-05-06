namespace D20Tek.Functional.Async;

public static class TaskExtensions
{
    public static async Task<TOut> ThenAsync<TIn, TOut>(this Task<TIn> task, Func<TIn, Task<TOut>> func) =>
        await func(await task);
}

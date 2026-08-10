namespace D20Tek.Functional.Async;

/// <summary>
/// Extension methods for <see cref="Task{T}"/> enabling fluent async pipelines.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Chains an async function onto a task, awaiting the task and passing the result to <paramref name="func"/>.
    /// Enables fluent composition of async operations.
    /// </summary>
    /// <typeparam name="TIn">The input task's result type.</typeparam>
    /// <typeparam name="TOut">The output type.</typeparam>
    /// <param name="task">The source task.</param>
    /// <param name="func">The async function to apply to the awaited result.</param>
    public static async Task<TOut> ThenAsync<TIn, TOut>(this Task<TIn> task, Func<TIn, Task<TOut>> func) =>
        await func(await task);
}

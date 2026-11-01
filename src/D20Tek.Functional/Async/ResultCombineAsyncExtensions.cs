namespace D20Tek.Functional.Async;

/// <summary>
/// Async applicative-style aggregation helpers for <see cref="Result{T}"/>. Every input task is awaited
/// before aggregation, so all errors are collected rather than short-circuiting on the first failure.
/// </summary>
public static class ResultCombineAsyncExtensions
{
    /// <summary>
    /// Awaits all supplied result tasks and combines them into a single result containing all success values,
    /// or a failure containing every collected error.
    /// </summary>
    /// <typeparam name="T">The success value type.</typeparam>
    /// <param name="results">The result tasks to combine.</param>
    public static async Task<Result<T[]>> CombineAsync<T>(params Task<Result<T>>[] results)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(results);

        var awaited = await Task.WhenAll(results);
        return Result.Combine(awaited);
    }

    /// <summary>
    /// Awaits two result tasks and projects their success values through <paramref name="combiner"/>.
    /// If either fails, all collected errors are returned.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="result1">The first result task.</param>
    /// <param name="result2">The second result task.</param>
    /// <param name="combiner">A function that combines both success values.</param>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, TResult>(
        Task<Result<T1>> result1,
        Task<Result<T2>> result2,
        Func<T1, T2, TResult> combiner)
        where T1 : notnull
        where T2 : notnull
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(result1);
        ArgumentNullException.ThrowIfNull(result2);

        return Result.Combine(await result1, await result2, combiner);
    }

    /// <summary>
    /// Awaits three result tasks and projects their success values through <paramref name="combiner"/>.
    /// If any fails, all collected errors are returned.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="result1">The first result task.</param>
    /// <param name="result2">The second result task.</param>
    /// <param name="result3">The third result task.</param>
    /// <param name="combiner">A function that combines all success values.</param>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, TResult>(
        Task<Result<T1>> result1,
        Task<Result<T2>> result2,
        Task<Result<T3>> result3,
        Func<T1, T2, T3, TResult> combiner)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(result1);
        ArgumentNullException.ThrowIfNull(result2);
        ArgumentNullException.ThrowIfNull(result3);

        return Result.Combine(await result1, await result2, await result3, combiner);
    }

    /// <summary>
    /// Awaits four result tasks and projects their success values through <paramref name="combiner"/>.
    /// If any fails, all collected errors are returned.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="T4">The fourth success value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="result1">The first result task.</param>
    /// <param name="result2">The second result task.</param>
    /// <param name="result3">The third result task.</param>
    /// <param name="result4">The fourth result task.</param>
    /// <param name="combiner">A function that combines all success values.</param>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, T4, TResult>(
        Task<Result<T1>> result1,
        Task<Result<T2>> result2,
        Task<Result<T3>> result3,
        Task<Result<T4>> result4,
        Func<T1, T2, T3, T4, TResult> combiner)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(result1);
        ArgumentNullException.ThrowIfNull(result2);
        ArgumentNullException.ThrowIfNull(result3);
        ArgumentNullException.ThrowIfNull(result4);

        return Result.Combine(await result1, await result2, await result3, await result4, combiner);
    }

    /// <summary>
    /// Awaits five result tasks and projects their success values through <paramref name="combiner"/>.
    /// If any fails, all collected errors are returned.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="T4">The fourth success value type.</typeparam>
    /// <typeparam name="T5">The fifth success value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="result1">The first result task.</param>
    /// <param name="result2">The second result task.</param>
    /// <param name="result3">The third result task.</param>
    /// <param name="result4">The fourth result task.</param>
    /// <param name="result5">The fifth result task.</param>
    /// <param name="combiner">A function that combines all success values.</param>
    public static async Task<Result<TResult>> CombineAsync<T1, T2, T3, T4, T5, TResult>(
        Task<Result<T1>> result1,
        Task<Result<T2>> result2,
        Task<Result<T3>> result3,
        Task<Result<T4>> result4,
        Task<Result<T5>> result5,
        Func<T1, T2, T3, T4, T5, TResult> combiner)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(result1);
        ArgumentNullException.ThrowIfNull(result2);
        ArgumentNullException.ThrowIfNull(result3);
        ArgumentNullException.ThrowIfNull(result4);
        ArgumentNullException.ThrowIfNull(result5);

        return Result.Combine(await result1, await result2, await result3, await result4, await result5, combiner);
    }

    /// <summary>
    /// Maps each element of <paramref name="source"/> through a failable async <paramref name="selector"/>
    /// and collects the outcomes. Returns all mapped values on success, or every collected error on failure.
    /// </summary>
    /// <typeparam name="T">The source element type.</typeparam>
    /// <typeparam name="TResult">The mapped success value type.</typeparam>
    /// <param name="source">The elements to map.</param>
    /// <param name="selector">An async function that maps an element to a result.</param>
    public static async Task<Result<TResult[]>> TraverseAsync<T, TResult>(
        IEnumerable<T> source,
        Func<T, Task<Result<TResult>>> selector)
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        var awaited = await Task.WhenAll(source.Select(selector));
        return Result.Combine(awaited);
    }

    /// <summary>
    /// Awaits a sequence of result tasks and collapses them into a single result containing all success
    /// values, or a failure containing every collected error.
    /// </summary>
    /// <typeparam name="T">The success value type.</typeparam>
    /// <param name="source">The result tasks to sequence.</param>
    public static async Task<Result<T[]>> SequenceAsync<T>(IEnumerable<Task<Result<T>>> source)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(source);

        var awaited = await Task.WhenAll(source);
        return Result.Combine(awaited);
    }
}

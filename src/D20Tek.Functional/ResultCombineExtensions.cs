namespace D20Tek.Functional;

/// <summary>
/// Applicative-style aggregation helpers for <see cref="Result{T}"/>. Unlike <see cref="Result{T}.Bind{TResult}"/>,
/// which short-circuits on the first failure, these methods evaluate every input and collect all errors so
/// callers can report multiple problems at once (for example, when validating several fields).
/// </summary>
public static partial class Result
{
    /// <summary>
    /// Combines multiple results into a single result containing all success values. If any input is a
    /// failure, returns a failure containing the errors collected from every failed input.
    /// </summary>
    /// <typeparam name="T">The success value type.</typeparam>
    /// <param name="results">The results to combine.</param>
    public static Result<T[]> Combine<T>(params Result<T>[] results)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(results);

        var errors = results.SelectMany(r => r.GetErrors()).ToArray();
        return errors.Length > 0
            ? Result<T[]>.Failure(errors)
            : Result<T[]>.Success([.. results.Select(r => r.GetValue())]);
    }

    /// <summary>
    /// Combines two results and projects their success values through <paramref name="combiner"/>.
    /// If either input fails, all collected errors are returned.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="combiner">A function that combines both success values.</param>
    public static Result<TResult> Combine<T1, T2, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Func<T1, T2, TResult> combiner)
        where T1 : notnull
        where T2 : notnull
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(combiner);

        var errors = CollectErrors(result1, result2);
        return errors.Length > 0
            ? Result<TResult>.Failure(errors)
            : Result<TResult>.Success(combiner(result1.GetValue(), result2.GetValue()));
    }

    /// <summary>
    /// Combines three results and projects their success values through <paramref name="combiner"/>.
    /// If any input fails, all collected errors are returned.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="combiner">A function that combines all success values.</param>
    public static Result<TResult> Combine<T1, T2, T3, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Func<T1, T2, T3, TResult> combiner)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(combiner);

        var errors = CollectErrors(result1, result2, result3);
        return errors.Length > 0
            ? Result<TResult>.Failure(errors)
            : Result<TResult>.Success(combiner(result1.GetValue(), result2.GetValue(), result3.GetValue()));
    }

    /// <summary>
    /// Combines four results and projects their success values through <paramref name="combiner"/>.
    /// If any input fails, all collected errors are returned.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="T4">The fourth success value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="combiner">A function that combines all success values.</param>
    public static Result<TResult> Combine<T1, T2, T3, T4, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Func<T1, T2, T3, T4, TResult> combiner)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(combiner);

        var errors = CollectErrors(result1, result2, result3, result4);
        return errors.Length > 0
            ? Result<TResult>.Failure(errors)
            : Result<TResult>.Success(
                combiner(result1.GetValue(), result2.GetValue(), result3.GetValue(), result4.GetValue()));
    }

    /// <summary>
    /// Combines five results and projects their success values through <paramref name="combiner"/>.
    /// If any input fails, all collected errors are returned.
    /// </summary>
    /// <typeparam name="T1">The first success value type.</typeparam>
    /// <typeparam name="T2">The second success value type.</typeparam>
    /// <typeparam name="T3">The third success value type.</typeparam>
    /// <typeparam name="T4">The fourth success value type.</typeparam>
    /// <typeparam name="T5">The fifth success value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="result1">The first result.</param>
    /// <param name="result2">The second result.</param>
    /// <param name="result3">The third result.</param>
    /// <param name="result4">The fourth result.</param>
    /// <param name="result5">The fifth result.</param>
    /// <param name="combiner">A function that combines all success values.</param>
    public static Result<TResult> Combine<T1, T2, T3, T4, T5, TResult>(
        Result<T1> result1,
        Result<T2> result2,
        Result<T3> result3,
        Result<T4> result4,
        Result<T5> result5,
        Func<T1, T2, T3, T4, T5, TResult> combiner)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(combiner);

        var errors = CollectErrors(result1, result2, result3, result4, result5);
        return errors.Length > 0
            ? Result<TResult>.Failure(errors)
            : Result<TResult>.Success(
                combiner(
                    result1.GetValue(),
                    result2.GetValue(),
                    result3.GetValue(),
                    result4.GetValue(),
                    result5.GetValue()));
    }

    /// <summary>
    /// Maps each element of <paramref name="source"/> through a failable <paramref name="selector"/> and
    /// collects the outcomes. Returns all mapped values on success, or every collected error on failure.
    /// </summary>
    /// <typeparam name="T">The source element type.</typeparam>
    /// <typeparam name="TResult">The mapped success value type.</typeparam>
    /// <param name="source">The elements to map.</param>
    /// <param name="selector">A function that maps an element to a result.</param>
    public static Result<TResult[]> Traverse<T, TResult>(
        IEnumerable<T> source,
        Func<T, Result<TResult>> selector)
        where TResult : notnull
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        return Combine(source.Select(selector).ToArray());
    }

    /// <summary>
    /// Collapses a sequence of results into a single result containing all success values,
    /// or a failure containing every collected error.
    /// </summary>
    /// <typeparam name="T">The success value type.</typeparam>
    /// <param name="source">The results to sequence.</param>
    public static Result<T[]> Sequence<T>(IEnumerable<Result<T>> source)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(source);

        return Combine(source.ToArray());
    }

    private static Error[] CollectErrors(params IResultMonad[] results) => [.. results.SelectMany(r => r.GetErrors())];
}

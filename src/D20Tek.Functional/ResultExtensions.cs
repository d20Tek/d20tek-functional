namespace D20Tek.Functional;

/// <summary>
/// Extension methods for <see cref="Result{T}"/>.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Flattens a nested <c>Result&lt;Result&lt;T&gt;&gt;</c> into a single <c>Result&lt;T&gt;</c>.
    /// </summary>
    /// <typeparam name="T">The inner success type.</typeparam>
    /// <param name="result">The nested result to flatten.</param>
    public static Result<T> Flatten<T>(this Result<Result<T>> result) where T : notnull =>
        result.Match(someResult => someResult, e => Result<T>.Failure(e));
}

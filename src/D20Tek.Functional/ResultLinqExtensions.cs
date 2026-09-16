namespace D20Tek.Functional;

/// <summary>
/// LINQ query syntax support for <see cref="Result{T}"/>. Implementing <c>Select</c> and <c>SelectMany</c>
/// enables C# query comprehension syntax (<c>from ... select ...</c>) over results, delegating to the
/// existing <see cref="Result{T}.Map{TResult}"/> and <see cref="Result{T}.Bind{TResult}"/> operators.
/// </summary>
public static class ResultLinqExtensions
{
    /// <summary>
    /// Projects the success value using <paramref name="selector"/>. Enables the <c>select</c> clause in
    /// LINQ query syntax and is equivalent to <see cref="Result{T}.Map{TResult}"/>.
    /// </summary>
    /// <typeparam name="T">The source success type.</typeparam>
    /// <typeparam name="TResult">The projected success type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="selector">A transform applied to the success value.</param>
    public static Result<TResult> Select<T, TResult>(this Result<T> result, Func<T, TResult> selector)
        where T : notnull
        where TResult : notnull =>
        result.Map(selector);

    /// <summary>
    /// Monadic projection that binds the success value to an intermediate result and then combines both
    /// values with <paramref name="resultSelector"/>. Enables multiple <c>from</c> clauses in LINQ query syntax
    /// and is equivalent to a <see cref="Result{T}.Bind{TResult}"/> followed by <see cref="Result{T}.Map{TResult}"/>.
    /// Errors from either result are propagated.
    /// </summary>
    /// <typeparam name="T">The source success type.</typeparam>
    /// <typeparam name="TIntermediate">The intermediate success type produced by <paramref name="selector"/>.</typeparam>
    /// <typeparam name="TResult">The final projected success type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="selector">A function producing an intermediate result from the success value.</param>
    /// <param name="resultSelector">A function combining the source and intermediate values.</param>
    public static Result<TResult> SelectMany<T, TIntermediate, TResult>(
        this Result<T> result,
        Func<T, Result<TIntermediate>> selector,
        Func<T, TIntermediate, TResult> resultSelector)
        where T : notnull
        where TIntermediate : notnull
        where TResult : notnull =>
        result.Bind(value => selector(value).Map(intermediate => resultSelector(value, intermediate)));

    /// <summary>
    /// Filters the result, keeping the success value only if it satisfies <paramref name="predicate"/>;
    /// otherwise produces a failure containing <paramref name="error"/>. Unlike <see cref="Optional{T}"/>,
    /// filtering a <see cref="Result{T}"/> requires an explicit error to represent the rejected value, so the
    /// error is never hidden or invented.
    /// </summary>
    /// <typeparam name="T">The success type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="predicate">A condition the success value must satisfy.</param>
    /// <param name="error">The error returned when the predicate is not satisfied.</param>
    public static Result<T> Where<T>(this Result<T> result, Func<T, bool> predicate, Error error)
        where T : notnull =>
        result.Bind(value => predicate(value) ? Result<T>.Success(value) : Result<T>.Failure(error));
}

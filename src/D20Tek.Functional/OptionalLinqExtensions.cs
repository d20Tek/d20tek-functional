namespace D20Tek.Functional;

/// <summary>
/// LINQ query syntax support for <see cref="Optional{T}"/>. Implementing <c>Select</c>, <c>SelectMany</c>,
/// and <c>Where</c> enables C# query comprehension syntax (<c>from ... select ...</c>) over optional values,
/// delegating to the existing <see cref="Optional{T}.Map{TResult}"/>, <see cref="Optional{T}.Bind{TResult}"/>,
/// and <see cref="Optional{T}.Filter"/> operators.
/// </summary>
public static class OptionalLinqExtensions
{
    /// <summary>
    /// Projects the contained value using <paramref name="selector"/>. Enables the <c>select</c> clause in
    /// LINQ query syntax and is equivalent to <see cref="Optional{T}.Map{TResult}"/>.
    /// </summary>
    /// <typeparam name="T">The source value type.</typeparam>
    /// <typeparam name="TResult">The projected value type.</typeparam>
    /// <param name="option">The source optional.</param>
    /// <param name="selector">A transform applied to the contained value.</param>
    public static Optional<TResult> Select<T, TResult>(this Optional<T> option, Func<T, TResult> selector)
        where T : notnull
        where TResult : notnull =>
        option.Map(selector);

    /// <summary>
    /// Monadic projection that binds the contained value to an intermediate optional and then combines both
    /// values with <paramref name="resultSelector"/>. Enables multiple <c>from</c> clauses in LINQ query syntax
    /// and is equivalent to a <see cref="Optional{T}.Bind{TResult}"/> followed by <see cref="Optional{T}.Map{TResult}"/>.
    /// </summary>
    /// <typeparam name="T">The source value type.</typeparam>
    /// <typeparam name="TIntermediate">The intermediate value type produced by <paramref name="selector"/>.</typeparam>
    /// <typeparam name="TResult">The final projected value type.</typeparam>
    /// <param name="option">The source optional.</param>
    /// <param name="selector">A function producing an intermediate optional from the source value.</param>
    /// <param name="resultSelector">A function combining the source and intermediate values.</param>
    public static Optional<TResult> SelectMany<T, TIntermediate, TResult>(
        this Optional<T> option,
        Func<T, Optional<TIntermediate>> selector,
        Func<T, TIntermediate, TResult> resultSelector)
        where T : notnull
        where TIntermediate : notnull
        where TResult : notnull =>
        option.Bind(value => selector(value).Map(intermediate => resultSelector(value, intermediate)));

    /// <summary>
    /// Filters the optional, keeping the value only if it satisfies <paramref name="predicate"/>.
    /// Enables the <c>where</c> clause in LINQ query syntax and is equivalent to <see cref="Optional{T}.Filter"/>.
    /// </summary>
    /// <typeparam name="T">The contained value type.</typeparam>
    /// <param name="option">The source optional.</param>
    /// <param name="predicate">A condition the value must satisfy to remain present.</param>
    public static Optional<T> Where<T>(this Optional<T> option, Func<T, bool> predicate)
        where T : notnull =>
        option.Filter(predicate);
}

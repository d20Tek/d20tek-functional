namespace D20Tek.Functional;

/// <summary>
/// Extension methods for <see cref="Optional{T}"/> and the non-generic <see cref="Optional"/> factory.
/// </summary>
public static class OptionalExtensions
{
    /// <summary>
    /// Converts a nullable value to an <see cref="Optional{T}"/>.
    /// Returns None if <paramref name="source"/> is <c>null</c>, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="source">The nullable value to wrap.</param>
    public static Optional<T> ToOptional<T>(this T? source) where T : notnull =>
        source is null ? Optional.None<T>() : Optional.Some<T>(source);

    /// <summary>
    /// Flattens a nested <c>Optional&lt;Optional&lt;T&gt;&gt;</c> into a single <c>Optional&lt;T&gt;</c>.
    /// </summary>
    /// <typeparam name="T">The inner value type.</typeparam>
    /// <param name="option">The nested optional to flatten.</param>
    public static Optional<T> Flatten<T>(this Optional<Optional<T>> option) where T : notnull =>
        option.Match(someOption => someOption, () => Optional<T>.None());

    /// <summary>
    /// Combines two optionals using <paramref name="mapper"/>. Returns None if either optional is empty.
    /// </summary>
    /// <typeparam name="T1">The first optional's value type.</typeparam>
    /// <typeparam name="T2">The second optional's value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="opt1">The first optional.</param>
    /// <param name="opt2">The second optional.</param>
    /// <param name="mapper">A function combining both values.</param>
    public static Optional<TResult> Map<T1, T2, TResult>(
        this Optional<T1> opt1,
        Optional<T2> opt2,
        Func<T1, T2, TResult> mapper)
        where T1 : notnull
        where T2 : notnull
        where TResult : notnull =>
        opt1.IsSome && opt2.IsSome ?
            Optional<TResult>.Some(mapper(opt1.Get(), opt2.Get())) :
            Optional<TResult>.None();

    /// <summary>
    /// Combines three optionals using <paramref name="mapper"/>. Returns None if any optional is empty.
    /// </summary>
    /// <typeparam name="T1">The first optional's value type.</typeparam>
    /// <typeparam name="T2">The second optional's value type.</typeparam>
    /// <typeparam name="T3">The third optional's value type.</typeparam>
    /// <typeparam name="TResult">The combined result type.</typeparam>
    /// <param name="opt1">The first optional.</param>
    /// <param name="opt2">The second optional.</param>
    /// <param name="opt3">The third optional.</param>
    /// <param name="mapper">A function combining all three values.</param>
    public static Optional<TResult> Map<T1, T2, T3, TResult>(
        this Optional<T1> opt1,
        Optional<T2> opt2,
        Optional<T3> opt3,
        Func<T1, T2, T3, TResult> mapper)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where TResult : notnull =>
        opt1.IsSome && opt2.IsSome && opt3.IsSome
            ? Optional<TResult>.Some(mapper(opt1.Get(), opt2.Get(), opt3.Get()))
            : Optional<TResult>.None();
}

/// <summary>
/// Non-generic factory methods for creating <see cref="Optional{T}"/> instances
/// and converting nullable/reference values to optionals.
/// </summary>
public static class Optional
{
    /// <summary>
    /// Creates an <see cref="Optional{T}"/> containing the specified value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to wrap.</param>
    public static Optional<T> Some<T>(T value) where T : notnull => new Some<T>(value);

    /// <summary>
    /// Creates an empty <see cref="Optional{T}"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public static Optional<T> None<T>() where T : notnull => new None<T>();

    /// <summary>
    /// Creates an optional from a nullable value type. Returns Some if the value has a value, None otherwise.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    /// <param name="value">The nullable struct to convert.</param>
    public static Optional<T> OfNullable<T>(Nullable<T> value) where T : struct =>
        value.HasValue ? Some(value.Value) : None<T>();

    /// <summary>
    /// Creates an optional from a reference type. Returns Some if non-null, None otherwise.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="obj">The object to convert.</param>
    public static Optional<T> OfObj<T>(T? obj) where T : class => obj != null ? Some(obj) : None<T>();
}
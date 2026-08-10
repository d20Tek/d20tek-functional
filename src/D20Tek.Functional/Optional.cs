using System.Collections.Immutable;

namespace D20Tek.Functional;

/// <summary>
/// A discriminated union representing an optional value: either <see cref="Some{T}"/> containing a value
/// or <see cref="None{T}"/> representing absence. This is the Option monad inspired by F#'s Option type,
/// providing a type-safe alternative to null references.
/// </summary>
/// <typeparam name="T">The type of the contained value.</typeparam>
public abstract class Optional<T> where T : notnull
{
    /// <summary>
    /// Creates an <see cref="Optional{T}"/> containing the specified value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    public static Optional<T> Some(T value) => new Some<T>(value);

    /// <summary>
    /// Creates an empty <see cref="Optional{T}"/> representing the absence of a value.
    /// </summary>
    public static Optional<T> None() => new None<T>();

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> into an <see cref="Optional{T}"/>.
    /// Null values become None; non-null values become Some.
    /// </summary>
    /// <param name="instance">The value to convert.</param>
    public static implicit operator Optional<T>(T instance) => instance.ToOptional();

    /// <summary>Gets whether this optional contains a value.</summary>
    public bool IsSome => this is Some<T>;

    /// <summary>Gets whether this optional is empty (no value).</summary>
    public bool IsNone => this is None<T>;

    /// <summary>
    /// Pattern-matches on the optional, invoking <paramref name="onSome"/> if a value is present
    /// or <paramref name="onNone"/> if empty.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match handlers.</typeparam>
    /// <param name="onSome">Function to invoke with the contained value.</param>
    /// <param name="onNone">Function to invoke when no value is present.</param>
    public abstract TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone);

    /// <summary>
    /// Monadic bind: if this optional contains a value, passes it to <paramref name="bind"/>
    /// which returns a new <see cref="Optional{TResult}"/>. If empty, returns None.
    /// </summary>
    /// <typeparam name="TResult">The type of the resulting optional's value.</typeparam>
    /// <param name="bind">A function that takes the value and produces a new optional.</param>
    public Optional<TResult> Bind<TResult>(Func<T, Optional<TResult>> bind) where TResult : notnull =>
        Match(v => bind(v), Optional<TResult>.None);

    /// <summary>
    /// Returns <c>true</c> if this optional contains a value equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    public bool Contains(T value) => Match(v => v.Equals(value), () => false);

    /// <summary>
    /// Returns 1 if this optional contains a value, 0 otherwise.
    /// </summary>
    public int Count() => Match(_ => 1, () => 0);

    /// <summary>
    /// Returns the contained value if present, otherwise returns <paramref name="defaultArg"/>.
    /// </summary>
    /// <param name="defaultArg">The fallback value when the optional is empty.</param>
    public T DefaultValue(T defaultArg) => Match(v => v, () => defaultArg);

    /// <summary>
    /// Returns the contained value if present, otherwise invokes <paramref name="func"/> to produce a fallback.
    /// </summary>
    /// <param name="func">A factory function invoked lazily when the optional is empty.</param>
    public T DefaultWith(Func<T> func) => Match(v => v, () => func());

    /// <summary>
    /// Returns <c>true</c> if the optional contains a value satisfying <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A condition to test against the contained value.</param>
    public bool Exists(Func<T, bool> predicate) => Match(v => predicate(v), () => false);

    /// <summary>
    /// Returns Some if the value satisfies <paramref name="predicate"/>, otherwise None.
    /// </summary>
    /// <param name="predicate">A condition the value must satisfy to remain present.</param>
    public Optional<T> Filter(Func<T, bool> predicate) =>
        Match(v => predicate(v) ? Optional<T>.Some(v) : Optional<T>.None(), Optional<T>.None);

    /// <summary>
    /// Applies a fold (left-aggregate) over the contained value starting from <paramref name="initial"/>.
    /// If the optional is empty, returns <paramref name="initial"/> unchanged.
    /// </summary>
    /// <typeparam name="TResult">The accumulator type.</typeparam>
    /// <param name="initial">The seed value for the fold.</param>
    /// <param name="func">A function that combines the accumulator with the value.</param>
    public TResult Fold<TResult>(TResult initial, Func<TResult, T, TResult> func) where TResult : notnull =>
        Match(v => func(initial, v), () => initial);

    /// <summary>
    /// Applies a right-fold over the contained value starting from <paramref name="initial"/>.
    /// If the optional is empty, returns <paramref name="initial"/> unchanged.
    /// </summary>
    /// <typeparam name="TResult">The accumulator type.</typeparam>
    /// <param name="initial">The seed value for the fold.</param>
    /// <param name="func">A function that combines the value with the accumulator.</param>
    public TResult FoldBack<TResult>(TResult initial, Func<T, TResult, TResult> func) where TResult : notnull =>
        Match(v => func(v, initial), () => initial);

    /// <summary>
    /// Returns <c>true</c> if the optional is empty or if the value satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A condition to test against the contained value.</param>
    public bool ForAll(Func<T, bool> predicate) => Match(v => predicate(v), () => true);

    /// <summary>
    /// Extracts the contained value. Throws <see cref="ArgumentNullException"/> if the optional is empty.
    /// Prefer <see cref="Match{TResult}"/> or <see cref="DefaultValue"/> for safe access.
    /// </summary>
    public T Get() => Match(v => v, () => throw Constants.ValueNullException);

    /// <summary>
    /// Executes a side-effect <paramref name="action"/> on the contained value (if present) and returns this optional unchanged.
    /// </summary>
    /// <param name="action">An action to execute on the value.</param>
    public Optional<T> Iter(Action<T> action)
    {
        if (IsSome) action(Get());
        return this;
    }

    /// <summary>
    /// Transforms the contained value using <paramref name="mapper"/>. If empty, returns None.
    /// Unlike <see cref="Bind{TResult}"/>, the mapper returns a raw value, not an Optional.
    /// </summary>
    /// <typeparam name="TResult">The type produced by the mapping function.</typeparam>
    /// <param name="mapper">A function to transform the contained value.</param>
    public Optional<TResult> Map<TResult>(Func<T, TResult> mapper) where TResult : notnull =>
        Match(v => Optional<TResult>.Some(mapper(v)), Optional<TResult>.None);

    /// <summary>
    /// Returns this optional if it contains a value, otherwise returns <paramref name="ifNone"/>.
    /// </summary>
    /// <param name="ifNone">An alternative optional to use when this one is empty.</param>
    public Optional<T> OrElse(Optional<T> ifNone) => Match(v => this, () => ifNone);

    /// <summary>
    /// Returns this optional if it contains a value, otherwise invokes <paramref name="ifNone"/> to produce an alternative.
    /// </summary>
    /// <param name="ifNone">A factory function for the alternative optional.</param>
    public Optional<T> OrElseWith(Func<Optional<T>> ifNone) => Match(v => this, () => ifNone());

    /// <summary>
    /// Returns a single-element array containing the value, or an empty array if empty.
    /// </summary>
    public T[] ToArray() => Match<T[]>(v => [v], () => []);

    /// <summary>
    /// Returns an immutable list containing the value, or an empty list if empty.
    /// </summary>
    public ImmutableList<T> ToList() => Match<ImmutableList<T>>(v => [v], () => []);

    /// <summary>
    /// Returns the contained value as a nullable reference, or <c>null</c> if empty.
    /// </summary>
    public T? ToNullable() => Match<T?>(v => v, () => default);

    /// <summary>
    /// Returns the contained value as a nullable reference, or <c>null</c> if empty.
    /// Alias for <see cref="ToNullable"/>.
    /// </summary>
    public T? ToObj() => Match<T?>(v => v, () => default);

    /// <inheritdoc/>
    public override string ToString() => 
        Match(v => Constants.SomeFormatString(typeof(T), v), () => Constants.NoneFormatString(typeof(T)));
}

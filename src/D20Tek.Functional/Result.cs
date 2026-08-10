using System.Collections.Immutable;

namespace D20Tek.Functional;

/// <summary>
/// A discriminated union representing either a successful value of type <typeparamref name="T"/> or
/// an array of <see cref="Error"/> instances. This is the core Result monad that enables
/// railway-oriented programming: chain operations with <see cref="Bind{TResult}"/> and <see cref="Map{TResult}"/>,
/// and resolve the final outcome with <see cref="Match{TResult}"/>.
/// </summary>
/// <typeparam name="T">The type of the success value.</typeparam>
public abstract class Result<T> : IResultMonad where T : notnull
{
    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <param name="value">The success value to wrap.</param>
    public static Result<T> Success(T value) => new Success<T>(value);

    /// <summary>
    /// Creates a failed result containing the specified errors.
    /// </summary>
    /// <param name="errors">One or more errors describing the failure.</param>
    public static Result<T> Failure(Error[] errors) => new Failure<T>(errors);

    /// <summary>
    /// Creates a failed result containing a single error.
    /// </summary>
    /// <param name="error">The error describing the failure.</param>
    public static Result<T> Failure(Error error) => new Failure<T>([error]);

    /// <summary>
    /// Creates a failed result from an exception, converting it to an <see cref="Error"/>.
    /// </summary>
    /// <param name="ex">The exception that caused the failure.</param>
    public static Result<T> Failure(Exception ex) => new Failure<T>([Error.Exception(ex)]);

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> into a successful <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="instance">The value to wrap as a success.</param>
    public static implicit operator Result<T>(T instance) => Success(instance);

    /// <summary>Gets whether this result represents a successful outcome.</summary>
    public bool IsSuccess => this is Success<T>;

    /// <summary>Gets whether this result represents a failed outcome.</summary>
    public bool IsFailure => this is Failure<T>;

    /// <summary>
    /// Pattern-matches on the result, invoking <paramref name="onSuccess"/> if successful
    /// or <paramref name="onFailure"/> if failed, and returns the produced value.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match handlers.</typeparam>
    /// <param name="onSuccess">Function to invoke with the success value.</param>
    /// <param name="onFailure">Function to invoke with the error array.</param>
    public abstract TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error[], TResult> onFailure);

    /// <summary>
    /// Monadic bind: if this result is successful, passes the value to <paramref name="bind"/>
    /// which returns a new <see cref="Result{TResult}"/>. If this result is a failure, propagates the errors.
    /// Use this to chain dependent operations that may themselves fail.
    /// </summary>
    /// <typeparam name="TResult">The success type of the resulting operation.</typeparam>
    /// <param name="bind">A function that takes the success value and produces a new result.</param>
    public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> bind) where TResult : notnull =>
        Match(v => bind(v), e => Result<TResult>.Failure(e));

    /// <summary>
    /// Returns 1 if this result is successful, 0 otherwise.
    /// </summary>
    public int Count() => Match(_ => 1, _ => 0);

    /// <summary>
    /// Returns the success value if present, otherwise returns <paramref name="defaultArg"/>.
    /// </summary>
    /// <param name="defaultArg">The fallback value when the result is a failure.</param>
    public T DefaultValue(T defaultArg) => Match(v => v, _ => defaultArg);

    /// <summary>
    /// Returns the success value if present, otherwise invokes <paramref name="func"/> to produce a fallback.
    /// </summary>
    /// <param name="func">A factory function invoked lazily when the result is a failure.</param>
    public T DefaultWith(Func<T> func) => Match(v => v, _ => func());

    /// <summary>
    /// Returns <c>true</c> if the result is successful and <paramref name="predicate"/> is satisfied.
    /// </summary>
    /// <param name="predicate">A condition to test against the success value.</param>
    public bool Exists(Func<T, bool> predicate) => Match(v => predicate(v), _ => false);

    /// <summary>
    /// Returns a successful result if the value satisfies <paramref name="predicate"/>;
    /// otherwise returns a failure with a "not found" filter error.
    /// </summary>
    /// <param name="predicate">A condition the success value must satisfy.</param>
    public Result<T> Filter(Func<T, bool> predicate) =>
        Match(
            v => predicate(v) ? Result<T>.Success(v) : Result<T>.Failure(Constants.ResultFilterError),
            Result<T>.Failure);

    /// <summary>
    /// Applies a fold (left-aggregate) over the success value starting from <paramref name="initial"/>.
    /// If the result is a failure, returns <paramref name="initial"/> unchanged.
    /// </summary>
    /// <typeparam name="TResult">The accumulator type.</typeparam>
    /// <param name="initial">The seed value for the fold.</param>
    /// <param name="func">A function that combines the accumulator with the success value.</param>
    public TResult Fold<TResult>(TResult initial, Func<TResult, T, TResult> func) where TResult : notnull =>
        Match(v => func(initial, v), _ => initial);

    /// <summary>
    /// Applies a right-fold over the success value starting from <paramref name="initial"/>.
    /// If the result is a failure, returns <paramref name="initial"/> unchanged.
    /// </summary>
    /// <typeparam name="TResult">The accumulator type.</typeparam>
    /// <param name="initial">The seed value for the fold.</param>
    /// <param name="func">A function that combines the success value with the accumulator.</param>
    public TResult FoldBack<TResult>(TResult initial, Func<T, TResult, TResult> func) where TResult : notnull =>
        Match(v => func(v, initial), _ => initial);

    /// <summary>
    /// Returns <c>true</c> if the result is a failure or if the success value satisfies <paramref name="predicate"/>.
    /// Equivalent to "for all values in this container, the predicate holds".
    /// </summary>
    /// <param name="predicate">A condition to test against the success value.</param>
    public bool ForAll(Func<T, bool> predicate) => Match(v => predicate(v), _ => true);

    /// <summary>
    /// Extracts the success value. Throws <see cref="ArgumentNullException"/> if the result is a failure.
    /// Prefer <see cref="Match{TResult}"/> or <see cref="DefaultValue"/> for safe access.
    /// </summary>
    public T GetValue() => Match(v => v, _ => throw Constants.ValueNullException);

    object? IResultMonad.GetValue() => Match<object?>(v => v, _ => null);

    /// <summary>
    /// Returns the error array if the result is a failure, or an empty array if successful.
    /// </summary>
    public Error[] GetErrors() => Match(_ => [], e => e);

    /// <summary>
    /// Executes a side-effect <paramref name="action"/> on the success value (if present) and returns this result unchanged.
    /// Useful for logging or other side-effects within a pipeline.
    /// </summary>
    /// <param name="action">An action to execute on the success value.</param>
    public Result<T> Iter(Action<T> action)
    {
        if (IsSuccess) action(GetValue());
        return this;
    }

    /// <summary>
    /// Transforms the success value using <paramref name="mapper"/>. If this result is a failure, propagates errors.
    /// Unlike <see cref="Bind{TResult}"/>, the mapper returns a raw value, not a Result.
    /// </summary>
    /// <typeparam name="TResult">The type produced by the mapping function.</typeparam>
    /// <param name="mapper">A function to transform the success value.</param>
    public Result<TResult> Map<TResult>(Func<T, TResult> mapper) where TResult : notnull =>
        Match(v => Result<TResult>.Success(mapper(v)), Result<TResult>.Failure);

    /// <summary>
    /// Transfers the error array from this result to a new <see cref="Result{TResult}"/> of a different type.
    /// Throws <see cref="InvalidOperationException"/> if called on a successful result.
    /// </summary>
    /// <typeparam name="TResult">The target result value type.</typeparam>
    public Result<TResult> MapErrors<TResult>() where TResult : notnull =>
        Match(_ => throw new InvalidOperationException(), Result<TResult>.Failure);

    /// <summary>
    /// Returns a single-element array containing the success value, or an empty array if failure.
    /// </summary>
    public T[] ToArray() => Match<T[]>(v => [v], _ => []);

    /// <summary>
    /// Returns an immutable list containing the success value, or an empty list if failure.
    /// </summary>
    public ImmutableList<T> ToList() => Match<ImmutableList<T>>(v => [v], _ => []);

    /// <summary>
    /// Converts this result to an <see cref="Optional{T}"/>: Some if successful, None if failure.
    /// Error information is discarded in the conversion.
    /// </summary>
    public Optional<T> ToOptional() => Match(Optional<T>.Some, _ => Optional<T>.None());

    /// <inheritdoc/>
    public override string ToString() =>
        Match(v => Constants.SuccessFormatString(typeof(T), v), e => Constants.FailureFormatString(typeof(T), e));
}

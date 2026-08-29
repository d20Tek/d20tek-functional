namespace D20Tek.Functional;

/// <summary>
/// Provides non-generic factory helpers for creating <see cref="Result{T}"/> instances,
/// with convenience methods for side-effect-only operations that produce a <see cref="Unit"/> value.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> of <see cref="Unit"/>, representing a
    /// side-effect operation that completed without a meaningful return value.
    /// </summary>
    public static Result<Unit> Success() => Result<Unit>.Success(Unit.Value);

    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> containing the specified value.
    /// The value type is inferred from <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="value">The success value to wrap.</param>
    public static Result<T> Success<T>(T value) where T : notnull => Result<T>.Success(value);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> of <see cref="Unit"/> containing the specified errors.
    /// </summary>
    /// <param name="errors">One or more errors describing the failure.</param>
    public static Result<Unit> Failure(Error[] errors) => Result<Unit>.Failure(errors);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> of <see cref="Unit"/> containing a single error.
    /// </summary>
    /// <param name="error">The error describing the failure.</param>
    public static Result<Unit> Failure(Error error) => Result<Unit>.Failure(error);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> of <see cref="Unit"/> from an exception.
    /// </summary>
    /// <param name="ex">The exception that caused the failure.</param>
    public static Result<Unit> Failure(Exception ex) => Result<Unit>.Failure(ex);
}

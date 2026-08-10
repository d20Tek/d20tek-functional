namespace D20Tek.Functional;

/// <summary>
/// Represents a failed <see cref="Result{T}"/> containing one or more <see cref="Error"/> instances.
/// </summary>
/// <typeparam name="T">The type parameter of the parent Result (not used, since this is a failure).</typeparam>
public sealed class Failure<T> : Result<T> where T : notnull
{
    private readonly Error[] _errors;

    internal Failure(Error[] errors) => _errors = errors;

    /// <inheritdoc/>
    public override TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error[], TResult> onFailure) =>
        onFailure(_errors);

    /// <summary>Implicitly converts an error array into a <see cref="Failure{T}"/>.</summary>
    public static implicit operator Failure<T>(Error[] errors) => new(errors);

    /// <summary>Implicitly converts a single error into a <see cref="Failure{T}"/>.</summary>
    public static implicit operator Failure<T>(Error error) => new([error]);

    /// <summary>Implicitly converts an exception into a <see cref="Failure{T}"/>.</summary>
    public static implicit operator Failure<T>(Exception ex) => new([Error.Exception(ex)]);
}

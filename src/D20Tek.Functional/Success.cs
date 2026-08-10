namespace D20Tek.Functional;

/// <summary>
/// Represents a successful <see cref="Result{T}"/> containing a value.
/// </summary>
/// <typeparam name="T">The type of the success value.</typeparam>
public sealed class Success<T> : Result<T> where T : notnull
{
    private readonly T _value;

    internal Success(T value) => _value = value;

    /// <inheritdoc/>
    public override TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error[], TResult> onFailure) =>
        onSuccess(_value);

    /// <summary>Implicitly wraps a value in a <see cref="Success{T}"/>.</summary>
    public static implicit operator Success<T>(T instance) => new(instance);

    /// <summary>Implicitly extracts the value from a <see cref="Success{T}"/>.</summary>
    public static implicit operator T(Success<T> instance) => instance._value;
}

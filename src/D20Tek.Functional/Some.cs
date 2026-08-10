namespace D20Tek.Functional;

/// <summary>
/// Represents the presence of a value in an <see cref="Optional{T}"/>.
/// This is the "has value" case of the Option monad.
/// </summary>
/// <typeparam name="T">The type of the contained value.</typeparam>
public sealed class Some<T> : Optional<T> where T : notnull
{
    private readonly T _value;

    internal Some(T value) => _value = value;

    /// <inheritdoc/>
    public override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) => onSome(_value);

    /// <summary>Implicitly wraps a value in a <see cref="Some{T}"/>.</summary>
    public static implicit operator Some<T>(T instance) => new(instance);

    /// <summary>Implicitly extracts the value from a <see cref="Some{T}"/>.</summary>
    public static implicit operator T(Some<T> instance) => instance._value;
}

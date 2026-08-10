namespace D20Tek.Functional;

/// <summary>
/// Represents the absence of a value in an <see cref="Optional{T}"/>.
/// This is the "empty" case of the Option monad.
/// </summary>
/// <typeparam name="T">The type parameter of the parent Optional.</typeparam>
public sealed class None<T> : Optional<T> where T : notnull
{
    internal None() { }

    /// <inheritdoc/>
    public override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) => onNone();
}

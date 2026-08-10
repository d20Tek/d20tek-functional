namespace D20Tek.Functional;

/// <summary>
/// Extension methods for <see cref="Identity{T}"/>.
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// Lifts a value into an <see cref="Identity{T}"/> monad.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="source">The value to wrap.</param>
    public static Identity<T> ToIdentity<T>(this T source) where T : notnull => Identity<T>.Create(source);

    /// <summary>
    /// Flattens a nested <c>Identity&lt;Identity&lt;T&gt;&gt;</c> into a single <c>Identity&lt;T&gt;</c>.
    /// </summary>
    /// <typeparam name="T">The inner value type.</typeparam>
    /// <param name="identity">The nested identity to flatten.</param>
    public static Identity<T> Flatten<T>(this Identity<Identity<T>> identity) where T : notnull => identity.Get();
}

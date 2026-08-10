namespace D20Tek.Functional;

/// <summary>
/// The Identity monad: a simple wrapper around a single non-null value of type <typeparamref name="T"/>.
/// Use Identity to lift plain values into a monadic pipeline, enabling uniform composition with
/// <see cref="Bind{TResult}"/>, <see cref="Map{TResult}"/>, and <see cref="Iter"/>.
/// </summary>
/// <typeparam name="T">The type of the wrapped value.</typeparam>
/// <param name="value">The value to wrap.</param>
public sealed class Identity<T>(T value) where T : notnull
{
    private readonly T _value = value;

    /// <summary>
    /// Monadic bind: passes the contained value to <paramref name="bind"/> which returns a new Identity.
    /// </summary>
    /// <typeparam name="TResult">The type of the new Identity's value.</typeparam>
    /// <param name="bind">A function producing a new Identity from the current value.</param>
    public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> bind) where TResult : notnull => bind(_value);

    /// <summary>
    /// Returns <c>true</c> if the contained value equals <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value to compare against.</param>
    public bool Contains(T value) => _value.Equals(value);

    /// <summary>
    /// Always returns 1, since Identity always contains exactly one value.
    /// </summary>
    public int Count() => 1;

    /// <summary>
    /// Returns <c>true</c> if the contained value satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A condition to test.</param>
    public bool Exists(Func<T, bool> predicate) => predicate(_value);

    /// <summary>
    /// Extracts the contained value.
    /// </summary>
    public T Get() => _value;

    /// <summary>
    /// Executes a side-effect <paramref name="action"/> on the contained value and returns this Identity unchanged.
    /// </summary>
    /// <param name="action">An action to execute.</param>
    public Identity<T> Iter(Action<T> action)
    {
        action(_value);
        return this;
    }

    /// <summary>
    /// Transforms the contained value using <paramref name="mapper"/> and wraps the result in a new Identity.
    /// </summary>
    /// <typeparam name="TResult">The type produced by the mapping function.</typeparam>
    /// <param name="mapper">A function to transform the value.</param>
    public Identity<TResult> Map<TResult>(Func<T, TResult> mapper) where TResult : notnull => new(mapper(_value));

    /// <inheritdoc/>
    public override string ToString() => Constants.IdentityFormatString(typeof(T), _value);

    /// <summary>
    /// Factory method to create a new <see cref="Identity{T}"/> containing the specified value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    public static Identity<T> Create(T value) => new(value);

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> into an <see cref="Identity{T}"/>.
    /// </summary>
    /// <param name="instance">The value to wrap.</param>
    public static implicit operator Identity<T>(T instance) => new(instance);

    /// <summary>
    /// Implicitly extracts the value from an <see cref="Identity{T}"/>.
    /// </summary>
    /// <param name="instance">The Identity to unwrap.</param>
    public static implicit operator T(Identity<T> instance) => instance._value;
}

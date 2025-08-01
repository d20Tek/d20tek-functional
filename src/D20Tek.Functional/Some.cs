namespace D20Tek.Functional;

[Obsolete("Use Optional.Some(value) instead.")]
public sealed class Some<T> : Option<T>
    where T : notnull
{
    private readonly T _value;

    internal Some(T value) => _value = value;

    public override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onSome(_value);

    public static implicit operator Some<T>(T instance) => new(instance);
    public static implicit operator T(Some<T> instance) => instance._value;
}

internal sealed class SomeOptional<T> : Optional<T>
    where T : notnull
{
    private readonly T _value;

    internal SomeOptional(T value) => _value = value;

    public override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onSome(_value);

    public static implicit operator SomeOptional<T>(T instance) => new(instance);
    public static implicit operator T(SomeOptional<T> instance) => instance._value;
}

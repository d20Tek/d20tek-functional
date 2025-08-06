namespace D20Tek.Functional;

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

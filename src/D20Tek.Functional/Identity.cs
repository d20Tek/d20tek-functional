namespace D20Tek.Functional;

public sealed class Identity<T>
    where T : notnull
{
    private readonly T _value;

    public Identity(T value) => _value = value;

    public Identity<TResult> Bind<TResult>(Func<T, Identity<TResult>> bind) where TResult : notnull => bind(_value);

    public bool Contains(T value) => _value.Equals(value);

    public int Count() => 1;

    public bool Exists(Func<T, bool> predicate) => predicate(_value);

    public T Get() => _value;

    public void Iter(Action<T> action) => action(_value);

    public Identity<TResult> Map<TResult>(Func<T, TResult> mapper) where TResult : notnull => new(mapper(_value));

    public override string ToString() => $"Identity<{typeof(T).Name}>(value = {_value})";

    public static Identity<T> Create(T value) => new(value);
    public static implicit operator Identity<T>(T instance) => new(instance);
    public static implicit operator T(Identity<T> instance) => instance._value;
}

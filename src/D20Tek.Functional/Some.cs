namespace D20Tek.Functional;

public sealed class Some<T> : Option<T>
    where T : notnull
{
    private readonly T _value;

    internal Some(T value) => _value = value;

    internal override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onSome(_value);
}

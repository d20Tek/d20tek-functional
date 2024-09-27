namespace D20Tek.Functional;

public sealed class Some<T> : Option<T>
    where T : notnull
{
    private readonly T _value
        ;

    public Some(T value) => _value = value;

    protected override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onSome(_value);
}

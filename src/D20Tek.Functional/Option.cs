namespace D20Tek.Functional;

public abstract class Option<T>
    where T: notnull
{
    // Factory methods
    public static Option<T> Some(T value) => new Some<T>(value);

    public static Option<T> None() => new None<T>();

    public bool IsSome => this is Some<T>;

    public bool IsNone => this is None<T>;

    protected abstract TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone);

    public virtual Option<TResult> Bind<TResult>(Func<T, Option<TResult>> bind) where TResult : notnull => 
        Match(v => bind(v), () => Option<TResult>.None());

    public bool Contains(T value) => Match(v => v.Equals(value), () => false);

    public int Count() => Match(_ => 1, () => 0);
}

public sealed class None<T> : Option<T>
    where T : notnull
{
    protected override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onNone();
}

public sealed class Some<T> : Option<T>
    where T : notnull
{
    private readonly T _value;

    public Some(T value) => _value = value;

    protected override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onSome(_value);
}

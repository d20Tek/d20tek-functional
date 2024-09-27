namespace D20Tek.Functional;

public abstract class Option<T>
    where T: notnull
{
    // Factory methods
    public static Option<T> Some(T value) => new Some<T>(value);

    public static Option<T> None() => new None<T>();

    public bool IsSome => this is Some<T>;

    public bool IsNone => this is None<T>;

    internal abstract TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone);

    public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> bind) where TResult : notnull =>  
        Match(v => bind(v), () => Option<TResult>.None());

    public bool Contains(T value) => Match(v => v.Equals(value), () => false);

    public int Count() => Match(_ => 1, () => 0);

    public T DefaultValue(T defaultArg) => Match(v => v, () => defaultArg);

    public T DefaultWith(Func<T> func) => Match(v => v, () => func());

    public bool Exists(Func<T, bool> predicate) => Match(v => predicate(v), () => false);

    public Option<T> Filter(Func<T, bool> predicate) => 
        Match(
            v => predicate(v) ? Option<T>.Some(v) : Option<T>.None(),
            () => Option<T>.None());

    public TResult Fold<TResult>(TResult initial, Func<TResult, T, TResult> func) where TResult : notnull =>
        Match(v => func(initial, v), () => initial);

    public TResult FoldBack<TResult>(TResult initial, Func<T, TResult, TResult> func) where TResult : notnull =>
        Match(v => func(v, initial), () => initial);

    public T Get() => Match(v => v, () => throw new ArgumentNullException("Value"));
}

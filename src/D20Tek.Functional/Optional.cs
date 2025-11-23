using System.Collections.Immutable;

namespace D20Tek.Functional;

public abstract class Optional<T> where T : notnull
{
    // Factory methods
    public static Optional<T> Some(T value) => new Some<T>(value);

    public static Optional<T> None() => new None<T>();

    public static implicit operator Optional<T>(T instance) => instance.ToOptional();

    public bool IsSome => this is Some<T>;

    public bool IsNone => this is None<T>;

    public abstract TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone);

    public Optional<TResult> Bind<TResult>(Func<T, Optional<TResult>> bind) where TResult : notnull =>
        Match(v => bind(v), () => Optional<TResult>.None());

    public bool Contains(T value) => Match(v => v.Equals(value), () => false);

    public int Count() => Match(_ => 1, () => 0);

    public T DefaultValue(T defaultArg) => Match(v => v, () => defaultArg);

    public T DefaultWith(Func<T> func) => Match(v => v, () => func());

    public bool Exists(Func<T, bool> predicate) => Match(v => predicate(v), () => false);

    public Optional<T> Filter(Func<T, bool> predicate) =>
        Match(v => predicate(v) ? Optional<T>.Some(v) : Optional<T>.None(), Optional<T>.None);

    public TResult Fold<TResult>(TResult initial, Func<TResult, T, TResult> func) where TResult : notnull =>
        Match(v => func(initial, v), () => initial);

    public TResult FoldBack<TResult>(TResult initial, Func<T, TResult, TResult> func) where TResult : notnull =>
        Match(v => func(v, initial), () => initial);

    public bool ForAll(Func<T, bool> predicate) => Match(v => predicate(v), () => true);

    public T Get() => Match(v => v, () => throw new ArgumentNullException("Value"));

    public Optional<T> Iter(Action<T> action)
    {
        if (IsSome) action(Get());
        return this;
    }

    public Optional<TResult> Map<TResult>(Func<T, TResult> mapper) where TResult : notnull =>
        Match(v => Optional<TResult>.Some(mapper(v)), Optional<TResult>.None);

    public Optional<T> OrElse(Optional<T> ifNone) => Match(v => this, () => ifNone);

    public Optional<T> OrElseWith(Func<Optional<T>> ifNone) => Match(v => this, () => ifNone());

    public T[] ToArray() => Match<T[]>(v => [v], () => []);

    public ImmutableList<T> ToList() => Match<ImmutableList<T>>(v => [v], () => []);

    public T? ToNullable() => Match<T?>(v => v, () => default);

    public T? ToObj() => Match<T?>(v => v, () => default);

    public override string ToString() =>
        Match(v => $"Some<{typeof(T).Name}>(value = {v})", () => $"None<{typeof(T).Name}>");
}

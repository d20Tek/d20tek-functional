namespace D20Tek.Functional;

public static class OptionalExtensions
{
    public static Optional<T> ToOptional<T>(this T? source) where T : notnull =>
        source is null ? Optional.None<T>() : Optional.Some<T>(source);

    public static Optional<T> Flatten<T>(this Optional<Optional<T>> option) where T : notnull =>
        option.Match(someOption => someOption, () => Optional<T>.None());

    public static Optional<TResult> Map<T1, T2, TResult>(
        this Optional<T1> opt1,
        Optional<T2> opt2,
        Func<T1, T2, TResult> mapper)
        where T1 : notnull
        where T2 : notnull
        where TResult : notnull =>
        opt1.IsSome && opt2.IsSome ?
            Optional<TResult>.Some(mapper(opt1.Get(), opt2.Get())) :
            Optional<TResult>.None();

    public static Optional<TResult> Map<T1, T2, T3, TResult>(
        this Optional<T1> opt1,
        Optional<T2> opt2,
        Optional<T3> opt3,
        Func<T1, T2, T3, TResult> mapper)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where TResult : notnull =>
        opt1.IsSome && opt2.IsSome && opt3.IsSome
            ? Optional<TResult>.Some(mapper(opt1.Get(), opt2.Get(), opt3.Get()))
            : Optional<TResult>.None();
}

public static class Optional
{
    public static Optional<T> Some<T>(T value) where T : notnull => new Some<T>(value);

    public static Optional<T> None<T>() where T : notnull => new None<T>();

    public static Optional<T> OfNullable<T>(Nullable<T> value) where T : struct =>
        value.HasValue ? Some(value.Value) : None<T>();

    public static Optional<T> OfObj<T>(T? obj) where T : class =>
        obj != null ? Some(obj) : None<T>();
}
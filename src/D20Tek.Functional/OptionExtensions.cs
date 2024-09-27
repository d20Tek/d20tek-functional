namespace D20Tek.Functional;

public static class OptionExtensions
{
    public static Option<T> ToOption<T>(this T? source) where T : notnull =>
        source is null ? Option.None<T>() : Option.Some<T>(source);

    public static Option<T> Flatten<T>(this Option<Option<T>> option) where T : notnull =>
        option.Match(someOption => someOption, () => Option<T>.None());

    public static Option<TResult> Map<T1, T2, TResult>(
        this Option<T1> opt1,
        Option<T2> opt2,
        Func<T1, T2, TResult> mapper)
        where T1 : notnull
        where T2 : notnull
        where TResult : notnull =>
        opt1.IsSome && opt2.IsSome ? Option<TResult>.Some(mapper(opt1.Get(), opt2.Get())) : Option<TResult>.None();

    public static Option<TResult> Map<T1, T2, T3, TResult>(
        this Option<T1> opt1,
        Option<T2> opt2,
        Option<T3> opt3,
        Func<T1, T2, T3, TResult> mapper)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where TResult : notnull =>
        opt1.IsSome && opt2.IsSome && opt3.IsSome
            ? Option<TResult>.Some(mapper(opt1.Get(), opt2.Get(), opt3.Get()))
            : Option<TResult>.None();
}

public static class Option
{
    public static Option<T> Some<T>(T value) where T : notnull => new Some<T>(value);

    public static Option<T> None<T>() where T : notnull => new None<T>();

    public static Option<T> OfNullable<T>(Nullable<T> value) where T : struct =>
        value.HasValue ? Some(value.Value) : None<T>();

    public static Option<T> OfObj<T>(T? obj) where T : class =>
        obj != null ? Some(obj) : None<T>();
}
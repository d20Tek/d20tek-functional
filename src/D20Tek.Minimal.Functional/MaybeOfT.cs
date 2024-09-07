namespace D20Tek.Minimal.Functional.New;

public abstract class Maybe<T>
{
    // Implicit conversion from T to Maybe<T>
    public static implicit operator Maybe<T>(T @this) => @this.ToMaybe();
}

public class Something<T> : Maybe<T>
{
    public T Value { get; init; }

    public Something(T value)
    {
        Value = value;
    }

    // Implicit conversion from T to Something<T>
    public static implicit operator Something<T>(T @this) => new Something<T>(@this);

    // Implicit conversion from Something<T> to T
    public static implicit operator T(Something<T> @this) => @this.Value;
}

public class Nothing<T> : Maybe<T>
{
}

public static class MaybeExtensions
{
    public static Maybe<T> ToMaybe<T>(this T source) => new Something<T>(source);

    public static Maybe<T> ToMaybeIfNull<T>(this T? source) =>
        source is null ? new Nothing<T>() : new Something<T>(source);

    public static Maybe<TToType> Bind<TFromType, TToType>(this Maybe<TFromType> maybe, Func<TFromType, TToType> f) =>
        maybe switch
        {
            Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default) =>
                TryExecute(() => f(sth.Value).ToMaybe()),
            _ => new Nothing<TToType>()
        };

    private static Maybe<TToType> TryExecute<TToType>(Func<Maybe<TToType>> func)
    {
        try
        {
            return func();
        }
        catch
        {
            return new Nothing<TToType>();
        }
    }

    public static Maybe<TToType> Map<TFromType, TToType>(this Maybe<TFromType> maybe, Func<TFromType, TToType> f) =>
        maybe switch
        {
            Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default) =>
                TryExecute(() => f(sth.Value)).ToMaybeIfNull(),
            _ => new Nothing<TToType>()
        };

    private static TToType? TryExecute<TToType>(Func<TToType> func)
    {
        try
        {
            return func();
        }
        catch
        {
            return default;
        }
    }
}


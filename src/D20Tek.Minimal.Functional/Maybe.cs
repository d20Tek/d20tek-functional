namespace D20Tek.Minimal.Functional;

public abstract class Maybe<T>
{
    public static implicit operator Maybe<T>(T @this) => @this.ToMaybe();
}

public class Something<T> : Maybe<T>
{
    public T Value { get; init; }

    public Something(T value)
    {
        Value = value;
    }

    public static implicit operator Something<T>(T @this) => new Something<T>(@this);
    public static implicit operator T(Something<T> @this) => @this.Value;
}

public class Nothing<T> : Maybe<T>
{
}

public class Error<T> : Maybe<T>
{
    public Error(Exception e)
    {
        ErrorMessage = e;
    }

    public Exception ErrorMessage { get; set; }
}

public static class MaybeExtensions
{
    public static Maybe<T> ToMaybe<T>(this T @this) => new Something<T>(@this);

    public static Maybe<TToType> Bind<TFromType, TToType>(this Maybe<TFromType> maybe, Func<TFromType, TToType> f)
    {
        switch (maybe)
        {
            case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                try
                {
                    return f(sth).ToMaybe();
                }
                catch (Exception e)
                {
                    return new Error<TToType>(e);
                }

            case Error<TFromType> err:
                return new Error<TToType>(err.ErrorMessage);

            default:
                return new Nothing<TToType>();
        }
    }
}
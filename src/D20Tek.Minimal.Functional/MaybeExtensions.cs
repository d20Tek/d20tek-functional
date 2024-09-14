namespace D20Tek.Minimal.Functional;

public static class MaybeExtensions
{
    public static Maybe<T> ToMaybe<T>(this T source) => new Something<T>(source);

    public static Maybe<T> ToMaybeIfNull<T>(this T? source) =>
        source is null ? new Nothing<T>() : new Something<T>(source);

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
                    return new Exceptional<TToType>(e);
                }

            case Exceptional<TFromType> err:
                return new Exceptional<TToType>(err.Exception);

            default:
                return new Nothing<TToType>();
        }
    }

    public static async Task<Maybe<TToType>> BindAsync<TFromType, TToType>(
        this Maybe<TFromType> source,
        Func<TFromType, Task<TToType>> f)
    {
        switch (source)
        {
            case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                try
                {
                    var result = await f(sth.Value);
                    return result.ToMaybe();
                }
                catch (Exception e)
                {
                    return new Exceptional<TToType>(e);
                }
            case Exceptional<TFromType> err:
                return new Exceptional<TToType>(err.Exception);
            default:
                return new Nothing<TToType>();
        }
    }

    public static async Task<Maybe<TToType>> BindAsync<TFromType, TToType>(
        this Maybe<TFromType> @this,
        Func<TFromType, ValueTask<TToType>> f)
    {
        switch (@this)
        {
            case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                try
                {
                    var result = await f(sth.Value);
                    return result.ToMaybe();
                }
                catch (Exception e)
                {
                    return new Exceptional<TToType>(e);
                }
            case Exceptional<TFromType> err:
                return new Exceptional<TToType>(err.Exception);
            default:
                return new Nothing<TToType>();
        }
    }

    public static Maybe<T> OnSomething<T>(this Maybe<T> @this, Action<T> act)
    {
        try
        {
            if (@this is Something<T> sth)
                act(sth.Value);
            return @this;
        }
        catch (Exception e)
        {
            return new Exceptional<T>(e);
        }
    }

    public static Maybe<T> OnNothing<T>(this Maybe<T> @this, Action act)
    {
        try
        {
            if (@this is Nothing<T> nth)
                act();
            return @this;
        }
        catch (Exception e)
        {
            return new Exceptional<T>(e);
        }
    }

    public static Maybe<T> OnError<T>(this Maybe<T> @this, Action<Exception> act)
    {
        try
        {
            if (@this is Exceptional<T> err)
                act(err.Exception);
            return @this;
        }
        catch (Exception e)
        {
            return new Exceptional<T>(e);
        }
    }
}

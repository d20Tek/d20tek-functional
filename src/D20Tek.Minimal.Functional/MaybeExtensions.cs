namespace D20Tek.Minimal.Functional;

public static class MaybeExtensions
{
    public static Maybe<T> ToMaybe<T>(this T source) => new Something<T>(source);

    public static Maybe<T> ToMaybeIfNull<T>(this T? source) =>
        source is null ? new Nothing<T>() : new Something<T>(source);

    public static Maybe<T> AsMaybe<T>(this Exceptional<T> ex) => (Maybe<T>)ex;

    public static Maybe<T> AsMaybe<T>(this Failure<T> failed) => (Maybe<T>)failed;

    public static Maybe<T> AsMaybe<T>(this Nothing<T> ex) => (Maybe<T>)ex;

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

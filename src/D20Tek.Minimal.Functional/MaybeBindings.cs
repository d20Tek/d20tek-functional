namespace D20Tek.Minimal.Functional;

public static class MaybeBindings
{
    public static Maybe<TToType> Bind<TFromType, TToType>(this Maybe<TFromType> maybe, Func<TFromType, TToType> f) =>
        maybe switch
        {
            Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default) =>
                Try(() => f(sth).ToMaybe(), e => new Exceptional<TToType>(e)),
            Exceptional<TFromType> ex => new Exceptional<TToType>(ex.Exception),
            Failure<TFromType> err => new Failure<TToType>(err.Error),
            _ => new Nothing<TToType>()
        };

    private static TToType Try<TToType>(Func<TToType> func, Func<Exception, TToType> onException)
    {
        try
        {
            return func();
        }
        catch (Exception e)
        {
            return onException(e);
        }
    }

    public static async Task<Maybe<TToType>> BindAsync<TFromType, TToType>(
        this Maybe<TFromType> maybe,
        Func<TFromType, Task<TToType>> f) =>
        maybe switch
        {
            Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default) =>
                await TryAsync(
                    () => f(sth.Value).ContinueWith(t => t.Result.ToMaybe()),
                    e => Task.FromResult(new Exceptional<TToType>(e).AsMaybe())),
            Exceptional<TFromType> ex => new Exceptional<TToType>(ex.Exception),
            Failure<TFromType> err => new Failure<TToType>(err.Error),
            _ => new Nothing<TToType>()
        };

    private static async Task<TToType> TryAsync<TToType>(Func<Task<TToType>> func, Func<Exception, Task<TToType>> onException)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            return await onException(e);
        }
    }
}

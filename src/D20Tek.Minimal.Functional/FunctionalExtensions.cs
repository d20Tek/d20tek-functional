namespace D20Tek.Minimal.Functional;

public static partial class FunctionalExtensions
{
    public static TOut Map<TIn, TOut>(this TIn instance, Func<TIn, TOut> func) =>
        func(instance);

    public static T Apply<T>(this T instance, Action<T> action)
    {
        action(instance);
        return instance;
    }

    public static TOut Fork<TIn, T1, T2, TOut>(
        this TIn instance,
        Func<TIn, T1> f1,
        Func<TIn, T2> f2,
        Func<T1, T2, TOut> fOut) =>
        fOut(f1(instance), f2(instance));

    public static void Fork<TIn, T1, T2>(
        this TIn instance,
        Func<TIn, T1> f1,
        Func<TIn, T2> f2,
        Action<T1, T2> fOut) =>
        fOut(f1(instance), f2(instance));

    public static TOut Alt<TIn, TOut>(this TIn instance, params Func<TIn, TOut>[] args) =>
        args.Select(x => x(instance)).First(x => x != null);

    public static TOut IfTrueOrElse<TOut>(this bool condition, Func<TOut> thenFunc, Func<TOut> elseFunc) =>
        condition ? thenFunc() : elseFunc();

    public static void IfTrueOrElse(this bool condition, Action thenAction, Action? elseAction = null)
    {
        if (condition)
            thenAction();
        else
            elseAction?.Invoke();
    }

    public static T IterateUntil<T>(this T instance, Func<T, T> updateFunction, Func<T, bool> endCondition)
    {
        var currentThis = instance;

        try
        {
            while (!endCondition(currentThis))
            {
                currentThis = updateFunction(currentThis);
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            throw;
        }

        return currentThis;
    }

    public static T TryExcept<T>(Func<T> operation, Func<Exception, T> onException)
    {
        try
        {
            return operation();
        }
        catch (Exception e)
        {
            return onException(e);
        }
    }
}

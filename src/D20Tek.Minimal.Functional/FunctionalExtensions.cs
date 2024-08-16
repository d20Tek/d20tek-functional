namespace D20Tek.Minimal.Functional;

public static partial class FunctionalExtensions
{
    public static TOut Map<TIn, TOut>(this TIn instance, Func<TIn, TOut> func) =>
        func(instance);

    public static TOut Alt<TIn, TOut>(this TIn instance, params Func<TIn, TOut>[] args) =>
        args.Select(x => x(instance)).First(x => x != null);

    public static T Tap<T>(this T instance, Action<T> action)
    {
        action(instance);
        return instance;
    }
}


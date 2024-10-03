namespace D20Tek.Functional;

public interface IState
{
}

public static class StateExtensions
{
    public static TOut Bind<TIn, TOut>(this TIn state, Func<TIn, TOut> bind)
        where TIn : IState
        where TOut : IState =>
        bind(state);


    public static T Iter<T>(this T state, Action<T> action)
        where T : IState
    {
        action(state);
        return state;
    }

    public static TOut Map<TIn, TOut>(this TIn state, Func<TIn, TOut> mapper)
        where TIn : IState
        where TOut : IState =>
        mapper(state);
}

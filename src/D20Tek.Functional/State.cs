namespace D20Tek.Functional;

/// <summary>
/// Marker interface for state objects that participate in the State monad pipeline.
/// Implement this interface on your immutable state records/classes to enable
/// <see cref="StateExtensions.Bind{TIn,TOut}"/>, <see cref="StateExtensions.Map{TIn,TOut}"/>,
/// and <see cref="StateExtensions.Iter{T}"/> extension methods.
/// </summary>
public interface IState { }

/// <summary>
/// Extension methods providing monadic operations (Bind, Map, Iter) for types implementing <see cref="IState"/>.
/// These enable functional pipelines over immutable state transitions.
/// </summary>
public static class StateExtensions
{
    /// <summary>
    /// Monadic bind: transforms the current state into a new state type using <paramref name="bind"/>.
    /// </summary>
    /// <typeparam name="TIn">The input state type.</typeparam>
    /// <typeparam name="TOut">The output state type.</typeparam>
    /// <param name="state">The current state.</param>
    /// <param name="bind">A function producing a new state from the current one.</param>
    public static TOut Bind<TIn, TOut>(this TIn state, Func<TIn, TOut> bind)
        where TIn : IState
        where TOut : IState =>
        bind(state);


    /// <summary>
    /// Executes a side-effect <paramref name="action"/> on the state and returns it unchanged.
    /// </summary>
    /// <typeparam name="T">The state type.</typeparam>
    /// <param name="state">The current state.</param>
    /// <param name="action">An action to execute on the state.</param>
    public static T Iter<T>(this T state, Action<T> action) where T : IState
    {
        action(state);
        return state;
    }

    /// <summary>
    /// Maps the current state to a non-state value using <paramref name="mapper"/>.
    /// Use this to extract a final result from a state pipeline.
    /// </summary>
    /// <typeparam name="TIn">The input state type.</typeparam>
    /// <typeparam name="TOut">The output type (does not need to implement IState).</typeparam>
    /// <param name="state">The current state.</param>
    /// <param name="mapper">A function producing the output value.</param>
    public static TOut Map<TIn, TOut>(this TIn state, Func<TIn, TOut> mapper)
        where TIn : IState
        where TOut : notnull =>
        mapper(state);
}

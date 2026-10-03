using System.Collections.Concurrent;

namespace D20Tek.Functional;

/// <summary>
/// Extension methods that memoize pure functions, caching results by their input arguments so repeated
/// calls with the same inputs avoid recomputation. Caches are backed by <see cref="ConcurrentDictionary{TKey, TValue}"/>
/// and <see cref="Lazy{T}"/> values to guarantee the wrapped function runs at most once per distinct input,
/// even under concurrent access.
/// </summary>
public static class MemoizeExtensions
{
    /// <summary>
    /// Wraps <paramref name="func"/> in a memoizing cache keyed by its single argument. Subsequent calls
    /// with an argument already seen return the cached result instead of invoking <paramref name="func"/> again.
    /// </summary>
    /// <typeparam name="T">The argument type, used as the cache key.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="func">The pure function to memoize.</param>
    public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        where T : notnull
    {
        var cache = new ConcurrentDictionary<T, Lazy<TResult>>();

        return arg => cache.GetOrAdd(
            arg,
            key => new Lazy<TResult>(() => func(key), LazyThreadSafetyMode.ExecutionAndPublication)).Value;
    }

    /// <summary>
    /// Wraps <paramref name="func"/> in a memoizing cache keyed by its two arguments.
    /// </summary>
    /// <typeparam name="T1">The first argument type.</typeparam>
    /// <typeparam name="T2">The second argument type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="func">The pure function to memoize.</param>
    public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        where T1 : notnull
        where T2 : notnull
    {
        var cache = new ConcurrentDictionary<(T1, T2), Lazy<TResult>>();

        return (arg1, arg2) => cache.GetOrAdd(
            (arg1, arg2),
            key => new Lazy<TResult>(() => func(key.Item1, key.Item2), LazyThreadSafetyMode.ExecutionAndPublication))
            .Value;
    }

    /// <summary>
    /// Wraps <paramref name="func"/> in a memoizing cache keyed by its three arguments.
    /// </summary>
    /// <typeparam name="T1">The first argument type.</typeparam>
    /// <typeparam name="T2">The second argument type.</typeparam>
    /// <typeparam name="T3">The third argument type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="func">The pure function to memoize.</param>
    public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        var cache = new ConcurrentDictionary<(T1, T2, T3), Lazy<TResult>>();

        return (arg1, arg2, arg3) => cache.GetOrAdd(
            (arg1, arg2, arg3),
            key => new Lazy<TResult>(
                () => func(key.Item1, key.Item2, key.Item3), LazyThreadSafetyMode.ExecutionAndPublication))
            .Value;
    }

    /// <summary>
    /// Wraps <paramref name="func"/> in a memoizing cache keyed by its four arguments.
    /// </summary>
    /// <typeparam name="T1">The first argument type.</typeparam>
    /// <typeparam name="T2">The second argument type.</typeparam>
    /// <typeparam name="T3">The third argument type.</typeparam>
    /// <typeparam name="T4">The fourth argument type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="func">The pure function to memoize.</param>
    public static Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        var cache = new ConcurrentDictionary<(T1, T2, T3, T4), Lazy<TResult>>();

        return (arg1, arg2, arg3, arg4) => cache.GetOrAdd(
            (arg1, arg2, arg3, arg4),
            key => new Lazy<TResult>(
                () => func(key.Item1, key.Item2, key.Item3, key.Item4), LazyThreadSafetyMode.ExecutionAndPublication))
            .Value;
    }

    /// <summary>
    /// Wraps <paramref name="func"/> in a memoizing cache keyed by its five arguments.
    /// </summary>
    /// <typeparam name="T1">The first argument type.</typeparam>
    /// <typeparam name="T2">The second argument type.</typeparam>
    /// <typeparam name="T3">The third argument type.</typeparam>
    /// <typeparam name="T4">The fourth argument type.</typeparam>
    /// <typeparam name="T5">The fifth argument type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="func">The pure function to memoize.</param>
    public static Func<T1, T2, T3, T4, T5, TResult> Memoize<T1, T2, T3, T4, T5, TResult>(
        this Func<T1, T2, T3, T4, T5, TResult> func)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
    {
        var cache = new ConcurrentDictionary<(T1, T2, T3, T4, T5), Lazy<TResult>>();

        return (arg1, arg2, arg3, arg4, arg5) => cache.GetOrAdd(
            (arg1, arg2, arg3, arg4, arg5),
            key => new Lazy<TResult>(
                () => func(key.Item1, key.Item2, key.Item3, key.Item4, key.Item5),
                LazyThreadSafetyMode.ExecutionAndPublication))
            .Value;
    }
}

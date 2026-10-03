using System.Collections.Concurrent;

namespace D20Tek.Functional.Async;

/// <summary>
/// Extension methods that memoize pure async functions, caching in-flight and completed results by their
/// input arguments so repeated calls with the same inputs avoid recomputation. Caches are backed by
/// <see cref="ConcurrentDictionary{TKey, TValue}"/> and <see cref="Lazy{T}"/> of <see cref="Task{TResult}"/>
/// to guarantee the wrapped function runs at most once per distinct input, even under concurrent access.
/// </summary>
public static class MemoizeAsyncExtensions
{
    /// <summary>
    /// Wraps <paramref name="func"/> in a memoizing cache keyed by its single argument. Concurrent calls with
    /// the same argument share the same in-flight task instead of invoking <paramref name="func"/> multiple
    /// times. Failed tasks are evicted from the cache so a later call can retry.
    /// </summary>
    /// <typeparam name="T">The argument type, used as the cache key.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="func">The pure async function to memoize.</param>
    public static Func<T, Task<TResult>> MemoizeAsync<T, TResult>(this Func<T, Task<TResult>> func)
        where T : notnull
    {
        var cache = new ConcurrentDictionary<T, Lazy<Task<TResult>>>();

        return async arg =>
        {
            var lazy = cache.GetOrAdd(
                arg,
                key => new Lazy<Task<TResult>>(() => func(key), LazyThreadSafetyMode.ExecutionAndPublication));

            try
            {
                return await lazy.Value;
            }
            catch
            {
                cache.TryRemove(arg, out _);
                throw;
            }
        };
    }

    /// <summary>
    /// Wraps <paramref name="func"/> in a memoizing cache keyed by its two arguments.
    /// </summary>
    /// <typeparam name="T1">The first argument type.</typeparam>
    /// <typeparam name="T2">The second argument type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="func">The pure async function to memoize.</param>
    public static Func<T1, T2, Task<TResult>> MemoizeAsync<T1, T2, TResult>(this Func<T1, T2, Task<TResult>> func)
        where T1 : notnull
        where T2 : notnull
    {
        var cache = new ConcurrentDictionary<(T1, T2), Lazy<Task<TResult>>>();

        return async (arg1, arg2) =>
        {
            var key = (arg1, arg2);
            var lazy = cache.GetOrAdd(
                key,
                k => new Lazy<Task<TResult>>(
                    () => func(k.Item1, k.Item2), LazyThreadSafetyMode.ExecutionAndPublication));

            try
            {
                return await lazy.Value;
            }
            catch
            {
                cache.TryRemove(key, out _);
                throw;
            }
        };
    }

    /// <summary>
    /// Wraps <paramref name="func"/> in a memoizing cache keyed by its three arguments.
    /// </summary>
    /// <typeparam name="T1">The first argument type.</typeparam>
    /// <typeparam name="T2">The second argument type.</typeparam>
    /// <typeparam name="T3">The third argument type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="func">The pure async function to memoize.</param>
    public static Func<T1, T2, T3, Task<TResult>> MemoizeAsync<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, Task<TResult>> func)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        var cache = new ConcurrentDictionary<(T1, T2, T3), Lazy<Task<TResult>>>();

        return async (arg1, arg2, arg3) =>
        {
            var key = (arg1, arg2, arg3);
            var lazy = cache.GetOrAdd(
                key,
                k => new Lazy<Task<TResult>>(
                    () => func(k.Item1, k.Item2, k.Item3), LazyThreadSafetyMode.ExecutionAndPublication));

            try
            {
                return await lazy.Value;
            }
            catch
            {
                cache.TryRemove(key, out _);
                throw;
            }
        };
    }
}

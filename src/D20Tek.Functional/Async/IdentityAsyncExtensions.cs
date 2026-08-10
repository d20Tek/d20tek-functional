namespace D20Tek.Functional.Async;

/// <summary>
/// Async extension methods for <see cref="Identity{T}"/> providing asynchronous Bind, Map, Iter, and Exists operations.
/// Each method has an overload accepting <c>Task&lt;Identity&lt;T&gt;&gt;</c> for pipeline chaining.
/// </summary>
public static class IdentityAsyncExtensions
{
    /// <summary>
    /// Asynchronous monadic bind on Identity.
    /// </summary>
    public static async Task<Identity<TResult>> BindAsync<T, TResult>(
        this Identity<T> identity, Func<T, Task<Identity<TResult>>> bind)
        where T: notnull
        where TResult : notnull =>
        await bind(identity.Get());

    /// <summary>
    /// Awaits an Identity task, then performs an asynchronous bind.
    /// </summary>
    public static async Task<Identity<TResult>> BindAsync<T, TResult>(
        this Task<Identity<T>> identity, Func<T, Task<Identity<TResult>>> bind)
        where T : notnull
        where TResult : notnull =>
        await (await identity).BindAsync(bind);

    /// <summary>
    /// Asynchronously tests whether the value satisfies a predicate.
    /// </summary>
    public static async Task<bool> ExistsAsync<T>(this Identity<T> identity, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await predicate(identity.Get());

    /// <summary>
    /// Awaits an Identity task, then asynchronously tests the predicate.
    /// </summary>
    public static async Task<bool> ExistsAsync<T>(this Task<Identity<T>> identity, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await (await identity).ExistsAsync(predicate);

    /// <summary>
    /// Executes an async side-effect on the Identity's value and returns the Identity unchanged.
    /// </summary>
    public static async Task<Identity<T>> IterAsync<T>(this Identity<T> identity, Func<T, Task> action)
        where T : notnull
    {
        await action(identity.Get());
        return identity;
    }

    /// <summary>
    /// Awaits an Identity task, then executes an async side-effect.
    /// </summary>
    public static async Task<Identity<T>> IterAsync<T>(this Task<Identity<T>> identity, Func<T, Task> action)
        where T : notnull =>
        await (await identity).IterAsync(action);

    /// <summary>
    /// Asynchronously maps the Identity's value to a new type.
    /// </summary>
    public static async Task<Identity<TResult>> MapAsync<T, TResult>(
        this Identity<T> identity, Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull =>
        new(await mapper(identity.Get()));

    /// <summary>
    /// Awaits an Identity task, then asynchronously maps its value.
    /// </summary>
    public static async Task<Identity<TResult>> MapAsync<T, TResult>(
        this Task<Identity<T>> identity, Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull =>
        await (await identity).MapAsync(mapper);
}

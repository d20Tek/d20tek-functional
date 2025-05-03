namespace D20Tek.Functional.Async;

public static class IdentityExtensions
{
    public static async Task<Identity<TResult>> BindAsync<T, TResult>(
        this Identity<T> identity,
        Func<T, Task<Identity<TResult>>> bind)
        where T: notnull
        where TResult : notnull =>
        await bind(identity.Get());

    public static async Task<Identity<TResult>> BindAsync<T, TResult>(
        this Task<Identity<T>> identity,
        Func<T, Task<Identity<TResult>>> bind)
        where T : notnull
        where TResult : notnull =>
        await (await identity).BindAsync(bind);

    public static async Task<bool> ExistsAsync<T>(this Identity<T> identity, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await predicate(identity.Get());

    public static async Task<bool> ExistsAsync<T>(this Task<Identity<T>> identity, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await (await identity).ExistsAsync(predicate);

    public static async Task<Identity<T>> IterAsync<T>(this Identity<T> identity, Func<T, Task> action)
        where T : notnull
    {
        await action(identity.Get());
        return identity;
    }

    public static async Task<Identity<T>> IterAsync<T>(this Task<Identity<T>> identity, Func<T, Task> action)
        where T : notnull =>
        await (await identity).IterAsync(action);

    public static async Task<Identity<TResult>> MapAsync<T, TResult>(
        this Identity<T> identity,
        Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull =>
        new(await mapper(identity.Get()));

    public static async Task<Identity<TResult>> MapAsync<T, TResult>(
        this Task<Identity<T>> identity,
        Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull =>
        await (await identity).MapAsync(mapper);
}

namespace D20Tek.Functional.Async;

public static class OptionalAsyncExtensions
{
    public static async Task<TResult> MatchAsync<T, TResult>(
        this Optional<T> option, Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone)
        where T : notnull
        where TResult : notnull =>
        option.IsSome ? await onSome(option.Get()) : await onNone();

    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<Optional<T>> option, Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone)
        where T : notnull
        where TResult : notnull =>
        await (await option).MatchAsync(onSome, onNone);

    public static async Task<Optional<TResult>> BindAsync<T, TResult>(
        this Optional<T> option, Func<T, Task<Optional<TResult>>> bind)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await bind(v), () => Task.FromResult(Optional<TResult>.None()));

    public static async Task<Optional<TResult>> BindAsync<T, TResult>(
        this Task<Optional<T>> option, Func<T, Task<Optional<TResult>>> bind)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await bind(v), () => Task.FromResult(Optional<TResult>.None()));

    public static async Task<T> DefaultWithAsync<T>(this Optional<T> option, Func<Task<T>> func)
        where T : notnull =>
        await option.MatchAsync(v => Task.FromResult(v), async () => await func());

    public static async Task<T> DefaultWithAsync<T>(this Task<Optional<T>> option, Func<Task<T>> func)
        where T : notnull =>
        await option.MatchAsync(v => Task.FromResult(v), async () => await func());

    public static async Task<bool> ExistsAsync<T>(this Optional<T> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(async (v) => await predicate(v), () => Task.FromResult(false));

    public static async Task<bool> ExistsAsync<T>(this Task<Optional<T>> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(async (v) => await predicate(v), () => Task.FromResult(false));

    public static async Task<Optional<T>> FilterAsync<T>(this Optional<T> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(
            async (v) => await predicate(v) ? Optional<T>.Some(v) : Optional<T>.None(),
            () => Task.FromResult(Optional<T>.None()));

    public static async Task<Optional<T>> FilterAsync<T>(this Task<Optional<T>> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(
            async (v) => await predicate(v) ? Optional<T>.Some(v) : Optional<T>.None(),
            () => Task.FromResult(Optional<T>.None()));

    public static async Task<TResult> FoldAsync<T, TResult>(
        this Optional<T> option, TResult initial, Func<TResult, T, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await func(initial, v), () => Task.FromResult(initial));

    public static async Task<TResult> FoldAsync<T, TResult>(
        this Task<Optional<T>> option, TResult initial, Func<TResult, T, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await func(initial, v), () => Task.FromResult(initial));

    public static async Task<TResult> FoldBackAsync<T, TResult>(
        this Optional<T> option, TResult initial, Func<T, TResult, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await func(v, initial), () => Task.FromResult(initial));

    public static async Task<TResult> FoldBackAsync<T, TResult>(
        this Task<Optional<T>> option, TResult initial, Func<T, TResult, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await func(v, initial), () => Task.FromResult(initial));

    public static async Task<bool> ForAllAsync<T>(this Optional<T> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(async (v) => await predicate(v), () => Task.FromResult(true));

    public static async Task<bool> ForAllAsync<T>(this Task<Optional<T>> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(async (v) => await predicate(v), () => Task.FromResult(true));

    public static async Task<Optional<T>> IterAsync<T>(this Optional<T> option, Func<T, Task> action)
        where T : notnull
    {
        if (option.IsSome) await action(option.Get());
        return option;
    }

    public static async Task<Optional<T>> IterAsync<T>(this Task<Optional<T>> option, Func<T, Task> action)
        where T : notnull =>
        await (await option).IterAsync(action);

    public static async Task<Optional<TResult>> MapAsync<T, TResult>(
        this Optional<T> option, Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(
            async (v) => Optional<TResult>.Some(await mapper(v)),
            () => Task.FromResult(Optional<TResult>.None()));

    public static async Task<Optional<TResult>> MapAsync<T, TResult>(
        this Task<Optional<T>> option, Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(
            async (v) => Optional<TResult>.Some(await mapper(v)),
            () => Task.FromResult(Optional<TResult>.None()));

    public static async Task<Optional<T>> OrElseWithAsync<T>(this Optional<T> option, Func<Task<Optional<T>>> ifNone)
        where T : notnull =>
        await option.MatchAsync(v => Task.FromResult(option), async () => await ifNone());

    public static async Task<Optional<T>> OrElseWithAsync<T>(
        this Task<Optional<T>> option, Func<Task<Optional<T>>> ifNone)
        where T : notnull =>
        await option.MatchAsync(v => option, async () => await ifNone());
}

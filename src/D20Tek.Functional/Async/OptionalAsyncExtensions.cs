namespace D20Tek.Functional.Async;

/// <summary>
/// Async extension methods for <see cref="Optional{T}"/> providing asynchronous Match, Bind, Map, Filter,
/// Fold, Iter, and other monadic operations. Each method has overloads for both <c>Optional&lt;T&gt;</c>
/// and <c>Task&lt;Optional&lt;T&gt;&gt;</c> to enable seamless async pipeline chaining.
/// </summary>
public static class OptionalAsyncExtensions
{
    /// <summary>
    /// Asynchronously pattern-matches an <see cref="Optional{T}"/>, invoking <paramref name="onSome"/> or <paramref name="onNone"/>.
    /// </summary>
    public static async Task<TResult> MatchAsync<T, TResult>(
        this Optional<T> option, Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone)
        where T : notnull
        where TResult : notnull =>
        option.IsSome ? await onSome(option.Get()) : await onNone();

    /// <summary>
    /// Asynchronously pattern-matches a <c>Task&lt;Optional&lt;T&gt;&gt;</c>, invoking <paramref name="onSome"/> or <paramref name="onNone"/>.
    /// </summary>
    public static async Task<TResult> MatchAsync<T, TResult>(
        this Task<Optional<T>> option, Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone)
        where T : notnull
        where TResult : notnull =>
        await (await option).MatchAsync(onSome, onNone);

    /// <summary>
    /// Asynchronously binds the value inside an <see cref="Optional{T}"/> to produce a new Optional.
    /// </summary>
    public static async Task<Optional<TResult>> BindAsync<T, TResult>(
        this Optional<T> option, Func<T, Task<Optional<TResult>>> bind)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await bind(v), () => Task.FromResult(Optional<TResult>.None()));

    /// <summary>
    /// Asynchronously binds the value inside a <c>Task&lt;Optional&lt;T&gt;&gt;</c> to produce a new Optional.
    /// </summary>
    public static async Task<Optional<TResult>> BindAsync<T, TResult>(
        this Task<Optional<T>> option, Func<T, Task<Optional<TResult>>> bind)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await bind(v), () => Task.FromResult(Optional<TResult>.None()));

    /// <summary>
    /// Returns the contained value if Some; otherwise asynchronously produces a default via <paramref name="func"/>.
    /// </summary>
    public static async Task<T> DefaultWithAsync<T>(this Optional<T> option, Func<Task<T>> func)
        where T : notnull =>
        await option.MatchAsync(v => Task.FromResult(v), async () => await func());

    /// <summary>
    /// Awaits the optional, then returns the contained value if Some; otherwise asynchronously produces a default.
    /// </summary>
    public static async Task<T> DefaultWithAsync<T>(this Task<Optional<T>> option, Func<Task<T>> func)
        where T : notnull =>
        await option.MatchAsync(v => Task.FromResult(v), async () => await func());

    /// <summary>
    /// Asynchronously tests whether the contained value satisfies <paramref name="predicate"/>. Returns false if None.
    /// </summary>
    public static async Task<bool> ExistsAsync<T>(this Optional<T> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(async (v) => await predicate(v), () => Task.FromResult(false));

    /// <summary>
    /// Awaits the optional, then asynchronously tests whether the contained value satisfies <paramref name="predicate"/>.
    /// </summary>
    public static async Task<bool> ExistsAsync<T>(this Task<Optional<T>> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(async (v) => await predicate(v), () => Task.FromResult(false));

    /// <summary>
    /// Asynchronously filters the optional, returning None if the predicate is not satisfied.
    /// </summary>
    public static async Task<Optional<T>> FilterAsync<T>(this Optional<T> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(
            async (v) => await predicate(v) ? Optional<T>.Some(v) : Optional<T>.None(),
            () => Task.FromResult(Optional<T>.None()));

    /// <summary>
    /// Awaits the optional, then asynchronously filters it, returning None if the predicate is not satisfied.
    /// </summary>
    public static async Task<Optional<T>> FilterAsync<T>(this Task<Optional<T>> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(
            async (v) => await predicate(v) ? Optional<T>.Some(v) : Optional<T>.None(),
            () => Task.FromResult(Optional<T>.None()));

    /// <summary>
    /// Asynchronously folds the optional value with an initial state using <paramref name="func"/>. Returns <paramref name="initial"/> if None.
    /// </summary>
    public static async Task<TResult> FoldAsync<T, TResult>(
        this Optional<T> option, TResult initial, Func<TResult, T, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await func(initial, v), () => Task.FromResult(initial));

    /// <summary>
    /// Awaits the optional, then asynchronously folds the value with an initial state.
    /// </summary>
    public static async Task<TResult> FoldAsync<T, TResult>(
        this Task<Optional<T>> option, TResult initial, Func<TResult, T, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await func(initial, v), () => Task.FromResult(initial));

    /// <summary>
    /// Asynchronously folds the optional value with reversed parameter order. Returns <paramref name="initial"/> if None.
    /// </summary>
    public static async Task<TResult> FoldBackAsync<T, TResult>(
        this Optional<T> option, TResult initial, Func<T, TResult, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await func(v, initial), () => Task.FromResult(initial));

    /// <summary>
    /// Awaits the optional, then asynchronously folds the value with reversed parameter order.
    /// </summary>
    public static async Task<TResult> FoldBackAsync<T, TResult>(
        this Task<Optional<T>> option, TResult initial, Func<T, TResult, Task<TResult>> func)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(async (v) => await func(v, initial), () => Task.FromResult(initial));

    /// <summary>
    /// Returns true if the optional is None or the value satisfies <paramref name="predicate"/> asynchronously.
    /// </summary>
    public static async Task<bool> ForAllAsync<T>(this Optional<T> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(async (v) => await predicate(v), () => Task.FromResult(true));

    /// <summary>
    /// Awaits the optional, then returns true if None or the value satisfies <paramref name="predicate"/>.
    /// </summary>
    public static async Task<bool> ForAllAsync<T>(this Task<Optional<T>> option, Func<T, Task<bool>> predicate)
        where T : notnull =>
        await option.MatchAsync(async (v) => await predicate(v), () => Task.FromResult(true));

    /// <summary>
    /// Asynchronously executes <paramref name="action"/> on the contained value if Some, then returns the original optional.
    /// </summary>
    public static async Task<Optional<T>> IterAsync<T>(this Optional<T> option, Func<T, Task> action)
        where T : notnull
    {
        if (option.IsSome) await action(option.Get());
        return option;
    }

    /// <summary>
    /// Awaits the optional, then asynchronously executes <paramref name="action"/> on the contained value if Some.
    /// </summary>
    public static async Task<Optional<T>> IterAsync<T>(this Task<Optional<T>> option, Func<T, Task> action)
        where T : notnull =>
        await (await option).IterAsync(action);

    /// <summary>
    /// Asynchronously transforms the contained value using <paramref name="mapper"/>, returning None if the optional is empty.
    /// </summary>
    public static async Task<Optional<TResult>> MapAsync<T, TResult>(
        this Optional<T> option, Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(
            async (v) => Optional<TResult>.Some(await mapper(v)),
            () => Task.FromResult(Optional<TResult>.None()));

    /// <summary>
    /// Awaits the optional, then asynchronously transforms the contained value using <paramref name="mapper"/>.
    /// </summary>
    public static async Task<Optional<TResult>> MapAsync<T, TResult>(
        this Task<Optional<T>> option, Func<T, Task<TResult>> mapper)
        where T : notnull
        where TResult : notnull =>
        await option.MatchAsync(
            async (v) => Optional<TResult>.Some(await mapper(v)),
            () => Task.FromResult(Optional<TResult>.None()));

    /// <summary>
    /// Returns the current optional if Some; otherwise asynchronously produces an alternative via <paramref name="ifNone"/>.
    /// </summary>
    public static async Task<Optional<T>> OrElseWithAsync<T>(this Optional<T> option, Func<Task<Optional<T>>> ifNone)
        where T : notnull =>
        await option.MatchAsync(v => Task.FromResult(option), async () => await ifNone());

    /// <summary>
    /// Awaits the optional, then returns it if Some; otherwise asynchronously produces an alternative.
    /// </summary>
    public static async Task<Optional<T>> OrElseWithAsync<T>(
        this Task<Optional<T>> option, Func<Task<Optional<T>>> ifNone)
        where T : notnull =>
        await option.MatchAsync(v => option, async () => await ifNone());
}

using D20Tek.Functional;

namespace BudgetTracker.Common;

internal static class ResultExtensions
{
    internal static void HandleResult<T>(this Result<T> result, Action<T> onSuccess, Action<string> onFailure)
        where T : notnull
    {
        if (result.IsSuccess)
            onSuccess(result.GetValue());
        else
            onFailure(result.GetErrors().First().ToString());
    }

    internal static async Task HandleResultAsync<T>(
        this Task<Result<T>> result, Action<T> onSuccess, Action<string> onFailure)
        where T : notnull =>
        (await result).HandleResult(onSuccess, onFailure);

    internal static void MatchAction<T>(this Optional<T> option, Action<T> onSome, Action onNone)
        where T : notnull
    {
        if (option.IsSome)
            onSome(option.Get());
        else
            onNone();
    }

    internal static async Task MatchActionAsync<T>(
        this Optional<T> option, Func<T, Task> onSome, Action onNone)
        where T : notnull
    {
        if (option.IsSome)
            await onSome(option.Get());
        else
            onNone();
    }
}

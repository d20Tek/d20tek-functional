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

    internal static void MatchAction<T>(this Optional<T> option, Action<T> onSome, Action onNone)
        where T : notnull
    {
        if (option.IsSome)
            onSome(option.Get());
        else
            onNone();
    }
}

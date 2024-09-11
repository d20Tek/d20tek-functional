using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class ResultPresenter
{
    public static void Render(
        this IAnsiConsole console,
        Maybe<WealthDataEntry> result,
        Func<Something<WealthDataEntry>, string> successMessage) =>
            console.MarkupLine(
                result switch
                {
                    Something<WealthDataEntry> s => successMessage(s),
                    Error<WealthDataEntry> e => $"{Constants.ErrorLabel} {e.ErrorMessage.Message}",
                    _ => Constants.UnexpectedErrorMesssage
                });
}

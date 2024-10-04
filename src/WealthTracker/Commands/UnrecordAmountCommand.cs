using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class UnrecordAmountCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Unrecord.Header))
             .Map(s => s.Repository.GetEntityById(s.Console.GetId())
                 .Iter(result => s.Console.DisplayResult<WealthDataEntry>(result, e => Constants.Unrecord.GetSuccessMessage(e)))
                 .Iter(result => PerformChange(s.Console, s.Repository, result))
                 .Map(_ => s with { Command = metadata.Name })).GetValue();

    private static void PerformChange(
        IAnsiConsole console,
        IWealthRepository repo,
        Result<WealthDataEntry> updateEntry) =>
            updateEntry.Iter(v => v.RemoveDailyValue(DateTimeOffset.Now))
                       .Map(entry => repo.Update(entry))
                       .Iter(result => console.DisplayResult(result, e => Constants.Unrecord.SuccessMessage(e)));

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.Unrecord.IdLabel));
}

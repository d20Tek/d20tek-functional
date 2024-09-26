using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;
using WealthTracker.Persistence;

namespace WealthTracker.Commands;

internal static class UnrecordPastAmountCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Unrecord.Header))
             .Map(s => s.Repository.GetEntityById(s.Console.GetId())
                 .Apply(result => s.Console.DisplayMaybe(
                     result, e => s.Console.WriteMessage(Constants.Unrecord.GetSuccessMessage(e))))
                 .Apply(result => PerformChange(s.Console, s.Repository, result))
                 .Map(_ => s with { Command = metadata.Name }));

    private static void PerformChange(
        IAnsiConsole console,
        IWealthRepository repo,
        Maybe<WealthDataEntry> updateEntry) =>
            updateEntry.OnSomething(
                v => v.Apply(v => v.RemoveDailyValue(console.GetDate()))
                      .Map(entry => repo.Update(entry))
                      .Apply(result => console.DisplayMaybe(
                          result, e => console.WriteMessage(Constants.Unrecord.SuccessMessage(e))))
                );

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.Unrecord.IdLabel));

    private static DateTimeOffset GetDate(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetPastDate(console, Constants.Unrecord.DateLabel);
}

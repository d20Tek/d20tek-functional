using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;
using WealthTracker.Persistence;

namespace WealthTracker.Commands;

internal static class RecordPastAmountCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Record.Header))
             .Map(s => s.Repository.GetWealthEntryById(s.Console.GetId())
                 .Apply(result => s.Console.DisplayMaybe(
                     result, e => s.Console.WriteMessage(Constants.Record.GetSuccessMessage(e))))
                 .Apply(result => PerformChange(s.Console, s.Repository, result))
                 .Map(_ => s with { Command = metadata.Name }));

    private static void PerformChange(
        IAnsiConsole console,
        IWealthRepository repo,
        Maybe<WealthDataEntry> updateEntry) =>
            updateEntry.OnSomething(
                v => v.Apply(v => v.AddDailyValue(console.GetDate(), console.GetAmount()))
                      .Map(entry => repo.Update(entry))
                      .Apply(result => console.DisplayMaybe(
                          result, e => console.WriteMessage(Constants.Record.SuccessMessage(e))))
                );

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.Record.IdLabel));

    private static DateTimeOffset GetDate(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetPastDate(console, Constants.Record.DateLabel);

    private static decimal GetAmount(this IAnsiConsole console) =>
        CurrencyComponent.Input(console, Constants.Record.AmountLabel);
}

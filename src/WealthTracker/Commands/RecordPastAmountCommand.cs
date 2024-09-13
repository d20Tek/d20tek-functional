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
                 .Apply(result => s.Console.Render(result, Constants.Record.GetSuccessMessage))
                 .Apply(result => PerformChange(s.Console, s.Repository, result))
                 .Map(_ => s with { Command = metadata.Name }));

    private static void PerformChange(
        IAnsiConsole console,
        IWealthRepository repo,
        Maybe<WealthDataEntry> updateEntry) =>
            updateEntry.OnSomething(
                v => v.Apply(v => v.AddDailyValue(console.GetDate(), console.GetAmount()))
                      .Map(entry => repo.Update(entry))
                      .Apply(result => console.Render(result, Constants.Record.SuccessMessage))
                );

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt(new TextPrompt<int>(Constants.Record.IdLabel));

    private static DateTimeOffset GetDate(this IAnsiConsole console) =>
        DateTimeOffsetPrompt.GetPastDate(console, Constants.Record.DateLabel);

    private static decimal GetAmount(this IAnsiConsole console) =>
        MoneyComponent.Input(console, Constants.Record.AmountLabel);
}

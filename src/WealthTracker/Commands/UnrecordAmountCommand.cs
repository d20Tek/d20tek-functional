using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;
using WealthTracker.Persistence;

namespace WealthTracker.Commands;

internal static class UnrecordAmountCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Unrecord.Header))
             .Map(s => s.Repository.GetWealthEntryById(s.Console.GetId())
                 .Apply(result => s.Console.Render(result, Constants.Unrecord.GetSuccessMessage))
                 .Apply(result => PerformChange(s.Console, s.Repository, result))
                 .Map(_ => s with { Command = metadata.Name }));

    private static void PerformChange(
        IAnsiConsole console,
        IWealthRepository repo,
        Maybe<WealthDataEntry> updateEntry) =>
            updateEntry.OnSomething(
                v => v.Apply(v => v.RemoveDailyValue(DateTimeOffset.Now))
                      .Map(entry => repo.Update(entry))
                      .Apply(result => console.Render(result, Constants.Unrecord.SuccessMessage))
                );

    private static int GetId(this IAnsiConsole console) =>
        console.Prompt<int>(new TextPrompt<int>(Constants.Unrecord.IdLabel));
}

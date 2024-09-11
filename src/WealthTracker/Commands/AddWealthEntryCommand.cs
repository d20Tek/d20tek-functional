using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class AddWealthEntryCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        new WealthDataEntry(0, GetName(), GetCategories())
            .Map(entry => state.Repository.Create(entry))
            .Apply(result => state.Console.MarkupLine(GetDisplayResult(result)))
            .Map(_ => state with { Command = metadata.Name });

    private static string GetName() => "name";

    private static string[] GetCategories() => ["category"];

    private static string GetDisplayResult(Maybe<WealthDataEntry> result) =>
        result switch
        {
            Something<WealthDataEntry> s => $"[green]Success:[/] The new account '{s.Value.Name}' was created.",
            Error<WealthDataEntry> e => $"[red]Error:[/] {e.ErrorMessage.Message}",
            _ => "[red]Error:[/] Unexpected error occurred."
        };
}

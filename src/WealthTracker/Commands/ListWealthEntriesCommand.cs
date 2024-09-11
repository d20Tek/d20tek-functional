using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class ListWealthEntriesCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.RenderEntries(s.Repository.GetWealthEntries()))
             .Map(s => s with { Command = metadata.Name });

    private static void RenderEntries(this IAnsiConsole console, WealthDataEntry[] entries)
    {
        console.WriteLine("List of Accounts");

        var table = ConfigureTable();
        table.AddRowsForEntries(entries);

        console.Write(table);
    }

    private static Table ConfigureTable() =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn("Id").Centered().Width(5),
                new TableColumn("Name").Width(50),
                new TableColumn("Categories").Width(40));

    private static void AddRowsForEntries(this Table table, WealthDataEntry[] entries) =>
        entries
            .Map(e => e.Any()
                ? entries.Select(entry => 
                    (entry.Id.ToString(),
                     entry.Name.CapOverflow(50),
                     entry.Categories.AsString().CapOverflow(40))).ToList()
                : [("", "No accounts are being tracked... please add some.", "")])
            .ForEach(x => table.AddRow(x.Item1, x.Item2, x.Item3));

    private static string AsString(this string[] categories) => string.Join(", ", categories);
}

using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class ListWealthEntriesCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.RenderEntries(s.Repository.GetWealthEntries()))
             .Map(s => s with { Command = metadata.Name });

    private static void RenderEntries(this IAnsiConsole console, WealthDataEntry[] entries) =>
        console.Apply(c => c.WriteLine(Constants.List.AccountListHeader))
               .Map(c => ConfigureTable().AddRowsForEntries(entries)
                   .Apply(table => c.Write(table)));

    private static Table ConfigureTable() =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnId).Centered().Width(Constants.List.ColumnIdLen),
                new TableColumn(Constants.List.ColumnName).Width(Constants.List.ColumnNameLen),
                new TableColumn(Constants.List.ColumnCategories).Width(Constants.List.ColumnCategoriesLen));

    private static Table AddRowsForEntries(this Table table, WealthDataEntry[] entries) =>
        entries
            .Map(e => e.Any()
                ? entries.Select(entry =>
                    (entry.Id.ToString(),
                     entry.Name.CapOverflow(50),
                     entry.Categories.AsString().CapOverflow(40))).ToList()
                : [(string.Empty, Constants.List.NoAccountsMessage, string.Empty)])
            .Apply(rows => rows.ForEach(x => table.AddRow(x.Item1, x.Item2, x.Item3)))
            .Map(_ => table);

    private static string AsString(this string[] categories) =>
        string.Join(Constants.JoinSeparator, categories);
}

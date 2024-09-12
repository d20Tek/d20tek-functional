using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class ListWealthEntriesCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteLine(Constants.List.AccountListHeader))
             .Apply(s => s.Console.Write(CreateTable(s.Repository.GetWealthEntries())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(WealthDataEntry[] entries) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnId).Centered().Width(Constants.List.ColumnIdLen),
                new TableColumn(Constants.List.ColumnName).Width(Constants.List.ColumnNameLen),
                new TableColumn(Constants.List.ColumnCategories).Width(Constants.List.ColumnCategoriesLen))
            .Apply(t => t.AddRowsForEntries(entries));

    private static void AddRowsForEntries(this Table table, WealthDataEntry[] entries) =>
        entries
            .Map(e => e.Any()
                ? entries.Select(entry => CreateRow(entry)).ToList()
                : [(string.Empty, Constants.List.NoAccountsMessage, string.Empty)])
            .Apply(rows => rows.ForEach(x => table.AddRow(x.Id, x.Name, x.Categories)));

    private static (string Id, string Name, string Categories) CreateRow(WealthDataEntry entry) =>
        (Id: entry.Id.ToString(),
         Name: entry.Name.CapOverflow(Constants.List.ColumnNameLen),
         Categories: entry.Categories.AsString().CapOverflow(Constants.List.ColumnCategoriesLen));
}

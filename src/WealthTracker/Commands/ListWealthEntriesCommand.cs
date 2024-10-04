using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class ListWealthEntriesCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.WriteLine(Constants.List.AccountListHeader))
             .Iter(s => s.Console.Write(CreateTable(s.Repository.GetEntities())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(WealthDataEntry[] entries) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnId).Centered().Width(Constants.List.ColumnIdLen),
                new TableColumn(Constants.List.ColumnName).Width(Constants.List.ColumnNameLen),
                new TableColumn(Constants.List.ColumnCategories).Width(Constants.List.ColumnCategoriesLen))
            .ToIdentity()
            .Iter(t => t.AddRowsForEntries(entries));

    private static void AddRowsForEntries(this Table table, WealthDataEntry[] entries) =>
        entries.ToIdentity()
            .Map(e => e.Any()
                ? entries.Select(entry => CreateRow(entry)).ToList()
                : [(string.Empty, Constants.List.NoAccountsMessage, string.Empty)])
            .Iter(rows => rows.ForEach(x => table.AddRow(x.Id, x.Name, x.Categories)));

    private static (string Id, string Name, string Categories) CreateRow(WealthDataEntry entry) =>
        (Id: entry.Id.ToString(),
         Name: entry.Name.CapOverflow(Constants.List.ColumnNameLen),
         Categories: entry.Categories.AsString().CapOverflow(Constants.List.ColumnCategoriesLen));
}

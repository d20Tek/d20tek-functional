using Apps.Common;
using D20Tek.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class CurrentNetWorthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.WriteLine(Constants.Current.ListHeader))
             .Iter(s => s.Console.Write(CreateTable(s.Repository.GetEntities())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(WealthDataEntry[] entries) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.Current.ColumnId).Centered().Width(Constants.Current.ColumnIdLen),
                new TableColumn(Constants.Current.ColumnName).Width(Constants.Current.ColumnNameLen),
                new TableColumn(Constants.Current.ColumnValue).RightAligned().Width(Constants.Current.ColumnValueLen))
            .ToIdentity()
            .Iter(t => t.AddRowsForEntries(entries));

    private static void AddRowsForEntries(this Table table, WealthDataEntry[] entries) =>
        entries.ToIdentity()
            .Map(e => e.Any()
                ? e.Select(entry => CreateRow(entry))
                   .Concat(CreateTotalRow(e.Sum(x => x.GetLatestValue())))
                   .ToList()
                : [(string.Empty, Constants.List.NoAccountsMessage, string.Empty)])
            .Iter(e => e.ForEach(x => table.AddRow(x.Id, x.Name, x.Value)));

    private static (string Id, string Name, string Value) CreateRow(WealthDataEntry entry) =>
        entry.GetLatestValue().ToIdentity()
            .Map(v => 
                (Id: entry.Id.ToString(),
                 Name: entry.Name.CapOverflow(Constants.Current.ColumnNameLen),
                 Value: CurrencyComponent.RenderWithNegative(v)));

    private static (string Id, string Name, string Value)[] CreateTotalRow(decimal total) =>
    [
        Constants.Current.TotalBorder,
        (Id: string.Empty,
         Name: Constants.Current.TotalLabel,
         Value: CurrencyComponent.RenderWithNegative(total))
    ];
}

using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class CurrentNetWorthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteLine(Constants.Current.ListHeader))
             .Apply(s => s.Console.Write(CreateTable(s.Repository.GetWealthEntries())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(WealthDataEntry[] entries) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.Current.ColumnId).Centered().Width(Constants.Current.ColumnIdLen),
                new TableColumn(Constants.Current.ColumnName).Width(Constants.Current.ColumnNameLen),
                new TableColumn(Constants.Current.ColumnValue).RightAligned().Width(Constants.Current.ColumnValueLen))
            .Apply(t => t.AddRowsForEntries(entries));

    private static void AddRowsForEntries(this Table table, WealthDataEntry[] entries) =>
        entries
            .Map(e => e.Any()
                ? e.Select(entry => CreateRow(entry))
                   .Concat(CreateTotalRow(e.Sum(x => x.GetLatestValue())))
                   .ToList()
                : [(string.Empty, Constants.List.NoAccountsMessage, string.Empty)])
            .ForEach(x => table.AddRow(x.Id, x.Name, x.Value));

    private static (string Id, string Name, string Value) CreateRow(WealthDataEntry entry) =>
        entry.GetLatestValue().Map(v => 
            (Id: entry.Id.ToString(),
             Name: entry.Name.CapOverflow(Constants.Current.ColumnNameLen),
             Value: MoneyPresenter.Render(v)));

    private static (string Id, string Name, string Value)[] CreateTotalRow(decimal total) =>
    [
        Constants.Current.TotalBorder,
        (Id: string.Empty,
         Name: Constants.Current.TotalLabel,
         Value: MoneyPresenter.Render(total))
    ];
}

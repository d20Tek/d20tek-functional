using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class MonthlyNetWorthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteLine(Constants.Monthly.ListHeader))
             .Apply(s => s.Console.Write(CreateTable(s.Repository.GetEntities(), GetDateRange())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(WealthDataEntry[] entries, DateTimeOffset[] dateRange) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(CreateColumns(dateRange))
            .Apply(t => t.AddRowsForEntries(entries, dateRange));

    private static void AddRowsForEntries(this Table table, WealthDataEntry[] entries, DateTimeOffset[] dateRange) =>
        entries
            .Map(e => e.Any()
                ? e.Select(entry => CreateRow(entry, dateRange))
                   .Concat(CreateTotalRow(e, dateRange))
                   .ToList()
                : [[string.Empty, Constants.Monthly.NoAccountsMessage]])
            .ForEach(x => table.AddRow(x));

    private static TableColumn[] CreateColumns(DateTimeOffset[] dateRange) =>
    [
        new TableColumn(Constants.Monthly.ColumnId).Centered().Width(Constants.Monthly.ColumnIdLen),
        new TableColumn(Constants.Monthly.ColumnName).Width(Constants.Monthly.ColumnNameLen),
        .. dateRange.Select(d => new TableColumn(d.ToMonthDay()).RightAligned()
                                                                .Width(Constants.Monthly.ColumnValueLen)),
        new TableColumn(Constants.Monthly.ColumnValue).RightAligned().Width(Constants.Monthly.ColumnValueLen)
    ];

    private static string[] CreateRow(WealthDataEntry entry, DateTimeOffset[] dateRange) =>
    [
        entry.Id.ToString(), entry.Name.CapOverflow(Constants.Monthly.ColumnNameLen),
        .. dateRange.Select(date => CurrencyComponent.RenderShort(entry.GetLatestValueFor(date))),
        CurrencyComponent.RenderShort(entry.GetLatestValue())
    ];

    private static string[][] CreateTotalRow(WealthDataEntry[] entries, DateTimeOffset[] dateRange) =>
    [
        Constants.Monthly.TotalBorder,
        [
            string.Empty,
            Constants.Monthly.TotalLabel,
            .. dateRange.Select(d => CurrencyComponent.RenderShort(entries.Sum(x => x.GetLatestValueFor(d)))),
            CurrencyComponent.RenderShort(entries.Sum(x => x.GetLatestValue())),
        ]
    ];

    private static DateTimeOffset[] GetDateRange() =>
        Enumerable.Range(0, Constants.Monthly.BackMonths)
            .Select(i => DateTimeOffset.Now.AddMonths(-(Constants.Monthly.BackMonths - 1) + i).StartOfMonth())
            .ToArray();
}

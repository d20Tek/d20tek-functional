using Apps.Common;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace WealthTracker.Commands;

internal static class YearlyNetWorthCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteLine(Constants.Yearly.ListHeader))
             .Apply(s => s.Console.Write(CreateTable(s.Repository.GetWealthEntries(), GetDateRange())))
             .Map(s => s with { Command = metadata.Name });

    private static Table CreateTable(WealthDataEntry[] entries, DateTimeOffset[] dateRange) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(CreateColumns())
            .Apply(t => t.AddRowsForEntries(entries, dateRange));

    private static TableColumn[] CreateColumns() =>
    [
        new TableColumn(Constants.Yearly.ColumnYear).Width(Constants.Yearly.ColumnYearLen),
        new TableColumn(Constants.Yearly.ColumnValue).RightAligned().Width(Constants.Yearly.ColumnValueLen),
        new TableColumn(Constants.Yearly.ColumnDelta).RightAligned().Width(Constants.Yearly.ColumnDeltaLen)
    ];

    private static void AddRowsForEntries(this Table table, WealthDataEntry[] entries, DateTimeOffset[] dateRange) =>
        dateRange.Select(d => CreateRow(entries.Sum(x => x.GetLatestValueFor(d)), d))
                 .Concat(CreateYtdRow(entries.Sum(x => x.GetLatestValue())))
                 .ToList()
                 .ForEach(r => table.AddRow(r.Year, r.Value, r.Delta));

    private static (string Year, string Value, string Delta) CreateRow(decimal yearlyTotal, DateTimeOffset date) =>
        (Year: date.Year.ToString(),
         Value: MoneyComponent.Render(yearlyTotal, string.Empty).CapOverflow(Constants.Yearly.ColumnValueLen),
         Delta: string.Empty);

    private static (string Year, string Value, string Delta)[] CreateYtdRow(decimal total) =>
    [
        Constants.Yearly.YtdBorder,
        (Year: Constants.Yearly.YtdLabel,
         Value: MoneyComponent.Render(total, string.Empty).CapOverflow(Constants.Yearly.ColumnValueLen),
         Delta: string.Empty)
    ];

    private static DateTimeOffset[] GetDateRange() =>
        Enumerable.Range(0, Constants.Yearly.BackYears)
            .Select(i => DateTimeOffset.Now.AddYears(-(Constants.Yearly.BackYears - 1) + i).StartOfYear())
            .ToArray();
}

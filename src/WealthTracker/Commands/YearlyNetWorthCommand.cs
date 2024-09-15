﻿using Apps.Common;
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
            .AddColumns(
                new TableColumn(Constants.Yearly.ColumnYear).Width(Constants.Yearly.ColumnYearLen),
                new TableColumn(Constants.Yearly.ColumnValue).RightAligned().Width(Constants.Yearly.ColumnValueLen),
                new TableColumn(Constants.Yearly.ColumnDelta).RightAligned().Width(Constants.Yearly.ColumnDeltaLen))
            .Apply(t => t.AddRowsForEntries(entries, dateRange));

    private static void AddRowsForEntries(this Table table, WealthDataEntry[] entries, DateTimeOffset[] dateRange) =>
        dateRange.Aggregate(
                    (PrevTotal: 0M, Rows: new List<(string Year, string Value, string Delta)>()),
                    (acc, date) =>
                        entries.Sum(x => x.GetLatestValueFor(date))
                               .Apply(currentTotal => acc.Rows.Add(CreateRow(currentTotal, acc.PrevTotal, date)))
                               .Map(currentTotal => (currentTotal, acc.Rows)),
                    acc => acc.Rows.Concat(CreateYtdRow(entries.Sum(x => x.GetLatestValue()), acc.PrevTotal)))
                 .ToList()
                 .ForEach(r => table.AddRow(r.Year, r.Value, r.Delta));

    private static (string Year, string Value, string Delta) CreateRow(
        decimal yearlyTotal,
        decimal prevTotal,
        DateTimeOffset date) =>
            (Year: date.ToDateString(),
             Value: MoneyComponent.Render(yearlyTotal, string.Empty).CapOverflow(Constants.Yearly.ColumnValueLen),
             Delta: MoneyComponent.Render(yearlyTotal - prevTotal, string.Empty)
                                  .CapOverflow(Constants.Yearly.ColumnDeltaLen));

    private static (string Year, string Value, string Delta)[] CreateYtdRow(decimal ytdTotal, decimal prevTotal) =>
    [
        (Year: Constants.Yearly.YtdLabel,
         Value: MoneyComponent.Render(ytdTotal, string.Empty).CapOverflow(Constants.Yearly.ColumnValueLen),
         Delta: MoneyComponent.Render(ytdTotal - prevTotal, string.Empty)
                              .CapOverflow(Constants.Yearly.ColumnDeltaLen))
    ];

    private static DateTimeOffset[] GetDateRange() =>
        Enumerable.Range(0, Constants.Yearly.BackYears)
            .Select(i => DateTimeOffset.Now.AddYears(-(Constants.Yearly.BackYears - 1) + i).StartOfYear())
            .ToArray();
}

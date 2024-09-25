using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ListSnapshotsCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.WriteMessage(Constants.List.ListHeader))
             .Apply(s => s.Console.Write(CreateTable(s.SnapshotRepo.GetEntities())))
             .Map(s => s with { Command = metadata.Name });

    internal static Table CreateTable(ReconciledSnapshot[] snapshots) =>
        new Table()
            .Border(TableBorder.Rounded)
            .AddColumns(
                new TableColumn(Constants.List.ColumnDate).Width(Constants.List.ColumnDateLen),
                new TableColumn(Constants.List.ColumnIncome).RightAligned().Width(Constants.List.ColumnIncomeLen),
                new TableColumn(Constants.List.ColumnBudget).RightAligned().Width(Constants.List.ColumnBudgetLen),
                new TableColumn(Constants.List.ColumnActual).RightAligned().Width(Constants.List.ColumnActualLen),
                new TableColumn(Constants.List.ColumnRemaining).RightAligned().Width(Constants.List.ColumnRemainingLen))
            .Apply(t => t.AddRowsForEntries(snapshots));

    private static void AddRowsForEntries(this Table table, ReconciledSnapshot[] snapshots) =>
        snapshots
            .Map(e => (e.Length != 0)
                ? snapshots.Select(entry => CreateRow(entry)).ToList()
                : [[string.Empty, Constants.List.NoSnapshotsMessage, string.Empty, string.Empty]])
            .Apply(rows => rows.ForEach(x => table.AddRow(x)));

    private static string[] CreateRow(ReconciledSnapshot snapshot) =>
    [
        snapshot.StartDate.ToDateString(),
        CurrencyComponent.Render(snapshot.TotalIncome.Amount),
        CurrencyComponent.Render(snapshot.TotalExpenses.Budget),
        CurrencyComponent.Render(snapshot.TotalExpenses.Actual),
        CurrencyComponent.Render(snapshot.TotalExpenses.Remaining)
    ];
}

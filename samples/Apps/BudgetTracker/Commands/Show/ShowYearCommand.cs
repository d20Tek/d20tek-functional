using Apps.Common;
using BudgetTracker.Entities;
using BudgetTracker.Persistence;
using D20Tek.Functional;
using Spectre.Console;

namespace BudgetTracker.Commands.Show;

internal static class ShowYearCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Iter(s => s.Console.DisplayHeader(Constants.Year.ListHeader))
             .Iter(s => s.DisplaySnapshotTable())
             .Map(_ => state with { Command = metadata.Name });

    private static void DisplaySnapshotTable(this AppState state) =>
        DateTimeOffset.Now.StartOfMonth().ToIdentity()
                      .Map(date => new DateRange(date.AddYears(-1), date))
                      .Map(range => state.SnapshotRepo.GetSnapshotsForDateRange(range))
                      .Iter(snapshots => state.Console.Write(
                          ListSnapshotsCommand.CreateTable(snapshots)
                                              .Pipe(t => t.AddTotalsRow(snapshots))));

    private static Table AddTotalsRow(this Table table, ReconciledSnapshot[] snapshots) => 
        table.ToIdentity()
             .Iter(t => t.AddRow(Constants.Year.TotalsSeparator))
             .Iter(t => t.AddRow(
                [
                    Constants.Year.Totals,
                    CurrencyComponent.Render(snapshots.Sum(x => x.TotalIncome.Amount)),
                    CurrencyComponent.Render(snapshots.Sum(x => x.TotalExpenses.Budget)),
                    CurrencyComponent.Render(snapshots.Sum(x => x.TotalExpenses.Actual)),
                    CurrencyComponent.Render(snapshots.Sum(x => x.TotalExpenses.Remaining))
                ]));
}

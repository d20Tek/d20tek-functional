using Apps.Common;
using BudgetTracker.Persistence;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Commands.Show;

internal static class ShowYearCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Year.ListHeader))
             .Map(s => DateTimeOffset.Now.StartOfMonth())
             .Apply(d => state.Console.Write(
                 ListSnapshotsCommand.CreateTable(
                     state.SnapshotRepo.GetSnapshotsForDateRange(d.AddYears(-1), d))))
             .Map(_ => state with { Command = metadata.Name });
}

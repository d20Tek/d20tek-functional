using Apps.Common;
using BudgetTracker.Persistence;
using D20Tek.Functional;

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
            .Iter(range => state.Console.Write(
                    ListSnapshotsCommand.CreateTable(state.SnapshotRepo.GetSnapshotsForDateRange(range))));
}

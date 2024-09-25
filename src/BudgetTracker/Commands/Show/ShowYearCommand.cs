using Apps.Common;
using BudgetTracker.Persistence;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Commands.Show;

internal static class ShowYearCommand
{
    public static AppState Handle(AppState state, CommandTypeMetadata metadata) =>
        state.Apply(s => s.Console.DisplayHeader(Constants.Year.ListHeader))
             .Map(_ => DateTimeOffset.Now.StartOfMonth())
             .Map(d => new DateRange(d.AddYears(-1), d)
             .Apply(range => state.Console.Write(
                 ListSnapshotsCommand.CreateTable(state.SnapshotRepo.GetSnapshotsForDateRange(range))))
             .Map(_ => state with { Command = metadata.Name }));
}

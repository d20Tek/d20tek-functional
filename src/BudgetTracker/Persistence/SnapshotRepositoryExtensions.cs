using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Persistence;

internal static class SnapshotRepositoryExtensions
{
    public static Maybe<ReconciledSnapshot> SnapshotNotFoundError =
        new Failure<ReconciledSnapshot>(Error.Validation(
            "Snapshot.NotFound",
            "There was no reconciled snapshot for the month that you specified."));

    public static bool SnapshotExists(
        this IReconciledSnapshotRepository snapRepo,
        DateTimeOffset date) =>
        snapRepo.GetEntities().FirstOrDefault(x => x.StartDate == date) is not null;

    public static Maybe<ReconciledSnapshot> GetSnapshotForMonth(
        this IReconciledSnapshotRepository snapRepo,
        DateTimeOffset date) =>
        snapRepo.GetEntities().FirstOrDefault(x => x.StartDate == date)
            .Map(snapshot => snapshot is null ? SnapshotNotFoundError : snapshot);

    public static ReconciledSnapshot[] GetSnapshotsForDateRange(
        this IReconciledSnapshotRepository snapRepo,
        DateTimeOffset start,
        DateTimeOffset end) =>
        snapRepo.GetEntities().Where(x => x.StartDate >= start && x.StartDate <= end)
                              .OrderBy(x => x.StartDate)
                              .ToArray() ?? [];
}

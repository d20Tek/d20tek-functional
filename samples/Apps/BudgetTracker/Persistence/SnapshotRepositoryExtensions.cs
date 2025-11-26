using Apps.Common;
using BudgetTracker.Entities;
using D20Tek.Functional;

namespace BudgetTracker.Persistence;

internal static class SnapshotRepositoryExtensions
{
    public static Result<ReconciledSnapshot> SnapshotNotFoundError =
        Result<ReconciledSnapshot>.Failure(Error.Validation(
            "Snapshot.NotFound",
            "There was no reconciled snapshot for the month that you specified."));

    public static bool SnapshotExists(this IReconciledSnapshotRepository snapRepo, DateTimeOffset date) =>
        snapRepo.GetEntities().FirstOrDefault(x => x.StartDate == date) is not null;

    public static Result<ReconciledSnapshot> GetSnapshotForMonth(
        this IReconciledSnapshotRepository snapRepo,
        DateTimeOffset date) =>
        snapRepo.GetEntities().FirstOrDefault(x => x.StartDate == date)
                .Pipe(snapshot => snapshot is null 
                               ? SnapshotNotFoundError
                               : Result<ReconciledSnapshot>.Success(snapshot));

    public static ReconciledSnapshot[] GetSnapshotsForDateRange(
        this IReconciledSnapshotRepository snapRepo,
        DateRange range) =>
        snapRepo.GetEntities().Where(x => range.InRange(x.StartDate))
                              .OrderBy(x => x.StartDate)
                              .ToArray() ?? [];
}

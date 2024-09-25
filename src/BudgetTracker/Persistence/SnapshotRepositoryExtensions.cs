using BudgetTracker.Entities;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Persistence;

internal static class SnapshotRepositoryExtensions
{
    public static bool SnapshotExists(
        this IReconciledSnapshotRepository snapRepo,
        DateTimeOffset date) =>
        snapRepo.GetEntities().FirstOrDefault(x => x.StartDate == date) is not null;

    public static Maybe<ReconciledSnapshot> GetSnapshotForMonth(
        this IReconciledSnapshotRepository snapRepo,
        DateTimeOffset date) =>
        snapRepo.GetEntities().FirstOrDefault(x => x.StartDate == date).ToMaybeIfNull();
}

namespace BudgetTracker.Persistence;

internal static class SnapshotRepositoryExtensions
{
    public static bool SnapshotExists(
        this IReconciledSnapshotRepository snapRepo,
        DateTimeOffset date) =>
        snapRepo.GetEntities().FirstOrDefault(x => x.StartDate == date) is not null;
}

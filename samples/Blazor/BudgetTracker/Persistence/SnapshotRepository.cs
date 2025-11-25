using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class SnapshotRepository(LowDb<BudgetDbDocument> db) : 
    LowDbRepository<ReconciledSnapshot, BudgetDbDocument>(db, s => s.CompletedSnapshots.Entities),
    IReconciledSnapshotRepository
{
    public Result<ReconciledSnapshot> GetSnapshotForMonth(DateTimeOffset date) =>
        Find(x => x.StartDate == date)
            .Map(x => x.First());

    public Result<IEnumerable<ReconciledSnapshot>> GetSnapshotsForDateRange(DateRange range) =>
        Find(x => range.InRange(x.StartDate))
            .Map(x => x.OrderBy(x => x.StartDate).AsEnumerable());
}

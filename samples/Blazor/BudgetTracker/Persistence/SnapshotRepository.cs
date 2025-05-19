using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class SnapshotRepository : LowDbRepository<ReconciledSnapshot, BudgetDbDocument>, IReconciledSnapshotRepository
{
    public SnapshotRepository(LowDb<BudgetDbDocument> db)
        : base(db, s => s.CompletedSnapshots.Entities)
    {
    }

    public Result<ReconciledSnapshot> GetSnapshotForMonth(DateTimeOffset date) =>
        Find(x => x.StartDate == date)
            .Map(x => x.First());

    public Result<IEnumerable<ReconciledSnapshot>> GetSnapshotsForDateRange(DateRange range) =>
        Find(x => range.InRange(x.StartDate))
            .Map(x => x.OrderBy(x => x.StartDate).AsEnumerable());
}

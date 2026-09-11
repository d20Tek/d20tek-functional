using BudgetTracker.Common;
using BudgetTracker.Domain;
using D20Tek.Functional;
using D20Tek.Functional.Async;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;

namespace BudgetTracker.Persistence;

internal class SnapshotRepository(LowDbAsync<BudgetDbDocument> db) : 
    LowDbAsyncRepository<ReconciledSnapshot, BudgetDbDocument>(db, s => s.CompletedSnapshots.Entities),
    IReconciledSnapshotRepository
{
    public Task<Result<ReconciledSnapshot>> GetSnapshotForMonth(DateTimeOffset date) =>
        FindAsync(x => x.StartDate == date)
            .MapAsync(x => Task.FromResult(x.First()));

    public Task<Result<IEnumerable<ReconciledSnapshot>>> GetSnapshotsForDateRange(DateRange range) =>
        FindAsync(x => range.InRange(x.StartDate))
            .MapAsync(x => Task.FromResult(x.OrderBy(x => x.StartDate).AsEnumerable()));
}

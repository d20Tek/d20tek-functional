using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;
using WealthTracker.Common;
using WealthTracker.Domain;

namespace WealthTracker.Persistence;

internal class WealthRepository : LowDbRepository<WealthDataEntity, WealthDbDocument>, IWealthRepository
{
    public WealthRepository(LowDb<WealthDbDocument> db)
        : base(db, w => w.Entities)
    {
    }
}

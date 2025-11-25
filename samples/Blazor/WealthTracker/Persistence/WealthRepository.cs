using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;
using WealthTracker.Common;
using WealthTracker.Domain;

namespace WealthTracker.Persistence;

internal class WealthRepository(LowDb<WealthDbDocument> db) : 
    LowDbRepository<WealthDataEntity, WealthDbDocument>(db, w => w.Entities), IWealthRepository
{
}

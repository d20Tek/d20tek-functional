using D20Tek.LowDb.Repositories;
using WealthTracker.Domain;

namespace WealthTracker.Persistence;

internal class WealthDbDocument : DbDocument
{
    public HashSet<WealthDataEntity> Entities { get; set; } = [];
}

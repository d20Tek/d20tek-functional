global using IWealthRepository = Apps.Repositories.IRepository<WealthTracker.Domain.WealthDataEntity>;

using Apps.Repositories;
using WealthTracker.Domain;

namespace WealthTracker.Persistence;

internal sealed class WealthDataStore : DataStoreElement<WealthDataEntity>
{
}

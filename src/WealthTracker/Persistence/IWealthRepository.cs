global using IWealthRepository = Apps.Repositories.IRepositoryOld<WealthTracker.WealthDataEntry>;

using Apps.Repositories;

namespace WealthTracker.Persistence;

internal sealed class WealthDataStore : DataStoreElement<WealthDataEntry>
{
}

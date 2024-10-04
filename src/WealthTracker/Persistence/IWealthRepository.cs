global using IWealthRepository = Apps.Repositories.IRepository<WealthTracker.WealthDataEntry>;

using Apps.Repositories;

namespace WealthTracker.Persistence;

internal sealed class WealthDataStore : DataStoreElement<WealthDataEntry>
{
}

using D20Tek.LowDb;
using D20Tek.Minimal.Functional;

namespace WealthTracker.Persistence;

internal class WealthFileRepository : IWealthRepository
{
    private readonly LowDb<WealthDataStore> _db;

    public WealthFileRepository(LowDb<WealthDataStore> db) => _db = db;

    public WealthDataEntry[] GetWealthEntries() =>
        _db.Get().Entities.ToArray() ?? [];

    public Maybe<WealthDataEntry> GetWealthEntryById(int id) =>
        _db.Get().Entities
            .Where(x => x.Id == id)
            .Select(entity => (Maybe<WealthDataEntry>)entity)
            .DefaultIfEmpty(Constants.NotFoundError(id))
            .First();
    
    public Maybe<WealthDataEntry> Create(WealthDataEntry entry) =>
        _db.Get()
            .Map(store => EnsureUniqueId(store)
                .Bind(id =>
                {
                    entry.SetId(id);
                    return Save(entry, () => store.Entities.Add(entry));
                }));

    public Maybe<WealthDataEntry> Update(WealthDataEntry entry) =>
        GetWealthEntryById(entry.Id).Bind(_ =>
            _db.Get()
                .Map(x => x.Entities.FindIndex(x => x.Id == entry.Id)
                    .Map(index => Save(entry, () => x.Entities[index] = entry))));

    public Maybe<WealthDataEntry> Delete(int id) =>
        GetWealthEntryById(id).Bind(e =>
            _db.Get()
                .Map(x => Save(e, () => x.Entities.RemoveAll(x => x.Id == id))));

    private WealthDataEntry Save(WealthDataEntry entry, Action op) =>
        entry.Apply(_ => op())
             .Apply(e => _db.Write());

    private static Maybe<int> EnsureUniqueId(WealthDataStore store) =>
        store.GetNextId()
            .Map(newId => (store.Entities.Any(x => x.Id == newId))
                       ? Constants.AlreadyExistsError(newId)
                       : newId.ToMaybe());
}

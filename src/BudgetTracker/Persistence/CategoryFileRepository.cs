using BudgetTracker.Entities;
using D20Tek.LowDb;
using D20Tek.Minimal.Functional;

namespace BudgetTracker.Persistence;

internal sealed class CategoryFileRepository : ICategoryRepository
{
    private readonly LowDb<BudgetDataStore> _db;

    public CategoryFileRepository(LowDb<BudgetDataStore> db) => _db = db;

    public BudgetCategory[] GetCategories() => _db.Get().Categories.ToArray() ?? [];

    public Maybe<BudgetCategory> GetCategoryById(int id) =>
        _db.Get().Categories
            .Where(x => x.Id == id)
            .Select(cat => (Maybe<BudgetCategory>)cat)
            .DefaultIfEmpty(Constants.NotFoundError<BudgetCategory>(id))
            .First();

    public Maybe<BudgetCategory> Create(BudgetCategory category) =>
        _db.Get()
            .Map(store => EnsureUniqueId(store)
                .Bind(id =>
                {
                    category.SetId(id);
                    return Save(category, () => store.Categories.Add(category));
                }));

    public Maybe<BudgetCategory> Delete(int id) =>
        GetCategoryById(id).Bind(e =>
            _db.Get()
                .Map(x => Save(e, () => x.Categories.RemoveAll(x => x.Id == id))));

    public Maybe<BudgetCategory> Update(BudgetCategory category) =>
        GetCategoryById(category.Id).Bind(_ =>
            _db.Get()
                .Map(x => x.Categories.FindIndex(x => x.Id == category.Id)
                    .Map(index => Save(category, () => x.Categories[index] = category))));

    private BudgetCategory Save(BudgetCategory entry, Action op) =>
        entry.Apply(_ => op())
             .Apply(e => _db.Write());

    private static Maybe<int> EnsureUniqueId(BudgetDataStore store) =>
        store.GetNextCategoryId()
            .Map(newId => (store.Categories.Any(x => x.Id == newId))
                       ? Constants.AlreadyExistsError(newId)
                       : newId.ToMaybe());
}

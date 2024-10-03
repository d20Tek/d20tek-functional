using D20Tek.Minimal.Functional;
using D20Tek.LowDb;

namespace Apps.Repositories;

public sealed class FileRepositoryOld<TEntity, TStore> : IRepositoryOld<TEntity>
    where TEntity : IEntity
    where TStore : class, new()
{
    public static Maybe<T> NotFoundError<T>(int id) =>
        new Failure<T>(Error.NotFound("Entity.NotFound", $"Entity with id={id} not found."));

    public static Maybe<int> AlreadyExistsError(int id) =>
        new Failure<int>(Error.Conflict("Entry.AlreadyExists", $"Entry with id={id} already exists."));

    private readonly LowDb<TStore> _db;
    private readonly Func<TStore, DataStoreElement<TEntity>> _elementAccessor;

    public FileRepositoryOld(LowDb<TStore> db, Func<TStore, DataStoreElement<TEntity>> elementAccessor)
    {
        _db = db;
        _elementAccessor = elementAccessor;
    }

    public TEntity[] GetEntities() => GetDataElement().Entities.ToArray() ?? [];

    public Maybe<TEntity> GetEntityById(int id) =>
        GetDataElement().Entities
            .Where(x => x.Id == id)
            .Select(entity => (Maybe<TEntity>)entity)
            .DefaultIfEmpty(NotFoundError<TEntity>(id))
            .First();

    public Maybe<TEntity> Create(TEntity entity) =>
        GetDataElement()
            .Map(store => EnsureUniqueId(store)
                .Bind(id =>
                {
                    entity.SetId(id);
                    return Save(entity, () => store.Entities.Add(entity));
                }));

    public Maybe<TEntity> Delete(int id) =>
        GetEntityById(id).Bind(e =>
            GetDataElement()
                .Map(x => Save(e, () => x.Entities.RemoveAll(x => x.Id == id))));

    public Maybe<TEntity[]> DeleteMany(TEntity[] entities) =>
        entities.Select(e => e.Id).ToMaybe()
            .Bind(ids =>
                GetDataElement()
                    .Apply(store => ids.Iter(id => store.Entities.RemoveAll(x => x.Id == id)))
                    .Apply(_ => _db.Write()))
                    .Map(_ => entities);


    public Maybe<TEntity> Update(TEntity entity) =>
        GetEntityById(entity.Id).Bind(_ =>
            GetDataElement()
                .Map(x => x.Entities.FindIndex(x => x.Id == entity.Id)
                    .Map(index => Save(entity, () => x.Entities[index] = entity))));

    private DataStoreElement<TEntity> GetDataElement() => _elementAccessor(_db.Get());

    private TEntity Save(TEntity entry, Action op) =>
        entry.Apply(_ => op())
             .Apply(e => _db.Write());

    private static Maybe<int> EnsureUniqueId(DataStoreElement<TEntity> element) =>
        element.GetNextId()
            .Map(newId => (element.Entities.Any(x => x.Id == newId)) ? AlreadyExistsError(newId) : newId.ToMaybe());
}

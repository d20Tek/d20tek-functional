using D20Tek.Functional;
using D20Tek.LowDb;

namespace Apps.Repositories;

public sealed class FileRepository<TEntity, TStore> : IRepository<TEntity>
    where TEntity : IEntity
    where TStore : class, new()
{
    public static Failure<T> NotFoundError<T>(int id) where T : notnull =>
        Error.NotFound("Entity.NotFound", $"Entity with id={id} not found.");

    public static Failure<int> AlreadyExistsError(int id) =>
        Error.Conflict("Entry.AlreadyExists", $"Entry with id={id} already exists.");

    private readonly LowDb<TStore> _db;
    private readonly Func<TStore, DataStoreElement<TEntity>> _elementAccessor;

    public FileRepository(LowDb<TStore> db, Func<TStore, DataStoreElement<TEntity>> elementAccessor)
    {
        _db = db;
        _elementAccessor = elementAccessor;
    }

    public TEntity[] GetEntities() => GetDataElement().Entities.ToArray() ?? [];

    public Result<TEntity> GetEntityById(int id) =>
        GetDataElement().Entities
            .Where(x => x.Id == id)
            .Select(entity => (Result<TEntity>)entity)
            .DefaultIfEmpty(NotFoundError<TEntity>(id))
            .First();

    public Result<TEntity> Create(TEntity entity) =>
        GetDataElement().ToIdentity()
            .Map(store => EnsureUniqueId(store)
                .Map(id =>
                {
                    entity.SetId(id);
                    return Save(entity, () => store.Entities.Add(entity));
                }));

    public Result<TEntity> Delete(int id) =>
        GetEntityById(id)
            .Map(e => Save(e, () => GetDataElement().Entities.RemoveAll(y => y.Id == id)));

    public Result<TEntity[]> DeleteMany(TEntity[] entities) =>
        Result<IEnumerable<int>>.Success(entities.Select(e => e.Id))
            .Map(ids =>
                GetDataElement().ToIdentity()
                    .Iter(store => ids.ForEach(id => store.Entities.RemoveAll(x => x.Id == id)))
                    .Iter(_ => _db.Write()))
                    .Map(_ => entities);

    public Result<TEntity> Update(TEntity entity) =>
        GetEntityById(entity.Id)
            .Map(_ => GetDataElement())
            .Map(store => Save(entity, () => store.Entities[GetEntityIndex(store, entity)] = entity));
                
    private DataStoreElement<TEntity> GetDataElement() => _elementAccessor(_db.Get());

    private TEntity Save(Identity<TEntity> entry, Action op) =>
        entry.Iter(_ => op())
             .Iter(e => _db.Write());

    private static Result<int> EnsureUniqueId(DataStoreElement<TEntity> element) =>
        element.GetNextId().ToIdentity()
            .Map(newId => (element.Entities.Any(x => x.Id == newId)) ? AlreadyExistsError(newId) : Result<int>.Success(newId));

    private static int GetEntityIndex(DataStoreElement<TEntity> store, TEntity entity) =>
        store.Entities.FindIndex(y => y.Id == entity.Id);
}

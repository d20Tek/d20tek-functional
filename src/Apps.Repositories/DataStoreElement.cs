namespace Apps.Repositories;

public sealed class DataStoreElement<TEntity>
{
    public int LastId { get; private set; }

    public List<TEntity> Entities { get; init; } = [];

    public DataStoreElement(int lastId, List<TEntity> entities)
    {
        LastId = lastId;
        Entities = entities;
    }

    public DataStoreElement()
        : this(0, [])
    {
    }

    public int GetNextId() => ++LastId;
}

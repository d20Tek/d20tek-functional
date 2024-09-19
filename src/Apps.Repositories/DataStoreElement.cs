namespace Apps.Repositories;

public sealed class DataStoreElement<TEntity>
{
    public int LastId { get; set; }

    public List<TEntity> Entities { get; init; } = [];

    public int GetNextId() => ++LastId;
}

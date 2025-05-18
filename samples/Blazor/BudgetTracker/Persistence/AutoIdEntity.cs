namespace BudgetTracker.Persistence;

public class AutoIdEntity<TEntity> where TEntity : IEntity
{
    public int LastId { get; set; }

    public HashSet<TEntity> Entities { get; init; } = [];

    public int GetNextId() => ++LastId;
}

public interface IEntity
{
    public int Id { get; }

    public void SetId(int id);
}

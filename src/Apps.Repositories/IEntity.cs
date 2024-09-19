namespace Apps.Repositories;

public interface IEntity
{
    public int Id { get; }

    public void SetId(int id);
}

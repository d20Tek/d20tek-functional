using D20Tek.Functional;

namespace Apps.Repositories;

public interface IRepository<TEntity>
    where TEntity : IEntity
{
    Result<TEntity> Create(TEntity entity);

    Result<TEntity> Delete(int id);

    Result<TEntity[]> DeleteMany(TEntity[] entities);

    TEntity[] GetEntities();

    Result<TEntity> GetEntityById(int id);

    Result<TEntity> Update(TEntity entity);
}

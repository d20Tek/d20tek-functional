using D20Tek.Minimal.Functional;

namespace Apps.Repositories;

public interface IRepository<TEntity>
    where TEntity : IEntity
{
    Maybe<TEntity> Create(TEntity entity);

    Maybe<TEntity> Delete(int id);

    Maybe<TEntity[]> DeleteMany(TEntity[] entities);

    TEntity[] GetEntities();

    Maybe<TEntity> GetEntityById(int id);

    Maybe<TEntity> Update(TEntity entity);
}

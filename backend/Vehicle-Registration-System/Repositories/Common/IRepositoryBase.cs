using System.Linq.Expressions;

namespace VehicleRegistrationSystem.Repositories.Common
{
    public interface IRepositoryBase<TEntity>
    {
        Task<TEntity?> GetByIdAsync(Guid id);

        Task<List<TEntity>> GetAllAsync();

        Task<bool> ExistsAsync(Expression<Func<TEntity,bool>> predicate);

        Task<List<TEntity>> FindAsync(Expression<Func<TEntity,bool>> predicate);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity> DeleteAsync(Guid id);
    }
}

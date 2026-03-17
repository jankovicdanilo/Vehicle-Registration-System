using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VehicleRegistrationSystem.Data;

namespace VehicleRegistrationSystem.Repositories.Common
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class 
    {
        protected readonly VehicleRegistrationDbContext appDbContext;
        protected readonly DbSet<TEntity> dbSet;

        public RepositoryBase(VehicleRegistrationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            dbSet = appDbContext.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity,bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity,bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            dbSet.Update(entity);
            await appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> DeleteAsync(Guid id)
        {
            TEntity entity = await dbSet.FindAsync(id);
            dbSet.Remove(entity);
            await appDbContext.SaveChangesAsync();
            return entity;
        }
    }
}

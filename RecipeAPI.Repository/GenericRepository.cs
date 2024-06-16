using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RecipeAPI.Repository
{
    public abstract class GenericRepository<T> where T : class
    {
        protected readonly RepositoryDbContext _dbContext;

        public GenericRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task InsertAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        }

        public virtual async Task InsertOrUpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            if(_dbContext.Database.IsSqlServer()) // BulkInsertOrUpdateAsync is not supported for InMemory database which was required for this project
                await _dbContext.BulkInsertOrUpdateAsync((IList<T>)entities, cancellationToken: cancellationToken);
            else
                await UpsertRangeAsync(entities, cancellationToken);
        }

        public virtual void MarkEntityAsModified(T entityToUpdate)
        {
            _dbContext.Set<T>().Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<T>().CountAsync(cancellationToken);
        }

        private async Task UpsertRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            foreach (var entity in entities)
            {
                var keyProperty = typeof(T).GetProperties()
                    .FirstOrDefault(prop => prop.GetCustomAttributes<KeyAttribute>().Any());

                if (keyProperty == null)
                {
                    throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have a property with the [Key] attribute.");
                }

                var keyValue = keyProperty.GetValue(entity);
                var existingEntity = await _dbContext.Set<T>().FindAsync(new object[] { keyValue }, cancellationToken);

                if (existingEntity == null)
                {
                    await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
                }
                else
                {
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
            }
        }
    }
}

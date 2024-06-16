using Microsoft.EntityFrameworkCore;
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;
using RecipeAPI.Repository.Contract;

namespace RecipeAPI.Repository
{
    public class PaginatedDataCacheRepository : GenericRepository<PaginatedDataCache>, IPaginatedDataCacheRepository
    {
        public PaginatedDataCacheRepository(RepositoryDbContext dbContext) : base(dbContext) { }

        public async Task<PaginatedDataCache> GetPaginatedResultAsync(PaginatedListArgs listArgs, string resourceName, CancellationToken cancellationToken)
        {
            return await _dbContext.PaginatedDataCache
                .FirstOrDefaultAsync(pr => pr.ResourceName == resourceName
                && pr.PageNumber == listArgs.PageNumber 
                && pr.ItemsPerPage == listArgs.PageSize);
        }

        public async Task InsertOrUpdateAsync(PaginatedDataCache paginatedResult, CancellationToken cancellationToken)
        {
            var existingResult = await _dbContext.PaginatedDataCache
                .FirstOrDefaultAsync(pr => pr.ResourceName == paginatedResult.ResourceName && pr.PageNumber == paginatedResult.PageNumber && pr.ItemsPerPage == paginatedResult.ItemsPerPage);

            if (existingResult != null)
            {
                existingResult.ResourceIds = paginatedResult.ResourceIds;
                existingResult.Total = paginatedResult.Total;
                existingResult.UpdatedAtUtc = DateTime.UtcNow;
                return;
            }

            await _dbContext.PaginatedDataCache.AddAsync(paginatedResult, cancellationToken);
        }
    }
}
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;

namespace RecipeAPI.Repository.Contract
{
    public interface IPaginatedDataCacheRepository
    {
        Task<PaginatedDataCache> GetPaginatedResultAsync(PaginatedListArgs listArgs, string resourceName, CancellationToken cancellationToken);
        Task InsertOrUpdateAsync(PaginatedDataCache paginatedResult, CancellationToken cancellationToken);
    }
}
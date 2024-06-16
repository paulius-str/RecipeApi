using RecipeAPI.Model.Entities;

namespace RecipeAPI.Service.Contract
{
    public interface IFavoriteRecipesService
    {
        Task AddAsync(int recipeId, CancellationToken cancellationToken);
        Task<RecipesPaginatedList> GetPaginatedListAsync(PaginatedListArgs listArgs, CancellationToken cancellationToken);
        Task RemoveAsync(int recipeId, CancellationToken cancellationToken);
    }
}
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;

namespace RecipeAPI.Repository.Contract
{
    public interface IFavoriteRecipeRepository
    {
        Task<IEnumerable<FavoriteRecipe>> GetNewestToOldestList(PaginatedListArgs listArgs, CancellationToken cancellationToken);
        Task<FavoriteRecipe> GetByRecipeIdAsync(int recipeId, CancellationToken cancellationToken);
        Task DeleteAsync(FavoriteRecipe favoriteRecipeToRemove, CancellationToken cancellationToken);
        Task InsertAsync(FavoriteRecipe favoriteRecipe, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
    }
}
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;

namespace RecipeAPI.Service.Contract
{
    public interface IRecipeClientService
    {
        Task<Recipe?> GetRecipeAsync(int id, CancellationToken cancellationToken);
        Task<RecipesPaginatedList?> GetRecipesAsync(PaginatedListArgs listArgs, CancellationToken cancellationToken);
    }
}
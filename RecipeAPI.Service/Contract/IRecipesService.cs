using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;

namespace RecipeAPI.Service.Contract
{
    public interface IRecipesService
    {
        Task<Recipe> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<RecipesPaginatedList> GetPaginatedListAsync(PaginatedListArgs listArgs, CancellationToken cancellationToken);
    }
}   
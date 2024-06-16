using RecipeAPI.Model.DataModel;

namespace RecipeAPI.Repository.Contract
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
        void MarkEntityAsModified(Recipe entityToTrack);
        Task<Recipe> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task InsertAsync(Recipe externalRecipe, CancellationToken cancellationToken);
        Task InsertOrUpdateRangeAsync(IEnumerable<Recipe> recipes, CancellationToken cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore;
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;
using RecipeAPI.Model.Exceptions;
using RecipeAPI.Repository.Contract;


namespace RecipeAPI.Repository
{
    public class FavoriteRecipeRepository : GenericRepository<FavoriteRecipe>, IFavoriteRecipeRepository
    {
        public FavoriteRecipeRepository(RepositoryDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<FavoriteRecipe>> GetNewestToOldestList(PaginatedListArgs listArgs, CancellationToken cancellationToken)
        {
            return await _dbContext.FavoriteRecipes
                .OrderByDescending(x => x.CreatedAtUtc)
                .Skip(listArgs.PageSize * (listArgs.PageNumber - 1))
                .Take(listArgs.PageSize)
                .Include(x => x.Recipe)
                .ToListAsync(cancellationToken);
        }

        public async Task<FavoriteRecipe> GetByRecipeIdAsync(int recipeId, CancellationToken cancellationToken)
        {
            return await _dbContext.FavoriteRecipes.Where(x => x.Recipe.Id == recipeId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task DeleteAsync(FavoriteRecipe favoriteRecipeToRemove, CancellationToken cancellationToken)
        {
            _dbContext.FavoriteRecipes.Remove(favoriteRecipeToRemove);
        }
    }
}

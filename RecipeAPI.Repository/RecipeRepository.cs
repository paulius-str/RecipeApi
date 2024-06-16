using Microsoft.EntityFrameworkCore;
using RecipeAPI.Model.DataModel;
using RecipeAPI.Repository.Contract;

namespace RecipeAPI.Repository
{
    public class RecipeRepository : GenericRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(RepositoryDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Recipe>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            return await GetRecipeQuery()
                .Where(r => ids.Contains(r.Id))
                .OrderBy(r => r.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<Recipe> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await GetRecipeQuery()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        private IQueryable<Recipe> GetRecipeQuery()
        {
            return _dbContext.Recipes
                .Select(r => new Recipe
                {
                    Id = r.Id,
                    Name = r.Name,
                    Image = r.Image,
                    Ingredients = r.Ingredients,
                    Instructions = r.Instructions,
                    PrepTimeMinutes = r.PrepTimeMinutes,
                    CookTimeMinutes = r.CookTimeMinutes,
                    Servings = r.Servings,
                    Difficulty = r.Difficulty,
                    Cuisine = r.Cuisine,
                    CaloriesPerServing = r.CaloriesPerServing,
                    Tags = r.Tags,
                    MealType = r.MealType,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    ReviewCount = r.ReviewCount,
                    UpdatedAtUtc = r.UpdatedAtUtc,
                    IsFavorite = _dbContext.FavoriteRecipes.Any(fr => fr.Recipe.Id == r.Id),
                });
        }
    }
}

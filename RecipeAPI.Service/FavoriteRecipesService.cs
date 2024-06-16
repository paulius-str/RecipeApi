using Microsoft.Extensions.Logging;
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;
using RecipeAPI.Model.Exceptions;
using RecipeAPI.Repository.Contract;
using RecipeAPI.Service.Contract;

namespace RecipeAPI.Services
{
    public class FavoriteRecipesService : IFavoriteRecipesService
    {
        private readonly IRecipeClientService _recipeClientService;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<RecipesService> _logger;

        public FavoriteRecipesService(
            IRecipeClientService recipeClientService,
            IRepositoryManager repositoryService,
            IDateTimeService dateTimeService,
            ILogger<RecipesService> logger
            )
        {
            _recipeClientService = recipeClientService;
            _repositoryManager = repositoryService;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public async Task AddAsync(int recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _repositoryManager.RecipeRepository.GetByIdAsync(recipeId, cancellationToken);

            if (recipe?.IsFavorite == true)
                throw new BadRequestException("Recipe is already in favorites");

            if (recipe == null)
            {
                recipe = await _recipeClientService.GetRecipeAsync(recipeId, cancellationToken);

                if (recipe == null)
                    throw new ResourceNotFoundException("Failed to fetch recipe from external and internal services");

                recipe.UpdatedAtUtc = _dateTimeService.UtcNow();
                await _repositoryManager.RecipeRepository.InsertAsync(recipe, cancellationToken);
            }

            var favoriteRecipe = new FavoriteRecipe() { RecipeId = recipe.Id, CreatedAtUtc = _dateTimeService.Now() };
            await _repositoryManager.FavoriteRecipeRepository.InsertAsync(favoriteRecipe, cancellationToken);
            await _repositoryManager.SaveChangesAsync(cancellationToken);
        }

        public async Task<RecipesPaginatedList> GetPaginatedListAsync(PaginatedListArgs listArgs, CancellationToken cancellationToken)
        {
            var favoriteRecipes = (await _repositoryManager.FavoriteRecipeRepository.GetNewestToOldestList(listArgs, cancellationToken)).Select(fr => fr.Recipe);

            foreach (var recipe in favoriteRecipes)
                recipe.IsFavorite = true;

            var total = await _repositoryManager.FavoriteRecipeRepository.CountAsync(cancellationToken);

            return new()
            {
                PageSize = listArgs.PageSize,
                PageNumber = listArgs.PageNumber,
                Recipes = favoriteRecipes,
                Total = total,
            };
        }

        public async Task RemoveAsync(int recipeId, CancellationToken cancellationToken)
        {
            var favoriteRecipe = await _repositoryManager.FavoriteRecipeRepository.GetByRecipeIdAsync(recipeId, cancellationToken);

            if (favoriteRecipe == null)
                throw new ResourceNotFoundException("Favorite recipe to delete not found");

            await _repositoryManager.FavoriteRecipeRepository.DeleteAsync(favoriteRecipe, cancellationToken);
            await _repositoryManager.SaveChangesAsync(cancellationToken);
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;
using RecipeAPI.Model.Exceptions;
using RecipeAPI.Repository.Contract;
using RecipeAPI.Service.Contract;

namespace RecipeAPI.Services
{
    public class RecipesService : IRecipesService
    {
        private readonly IRecipeClientService _recipeClientService;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<RecipesService> _logger;
        private readonly IConfiguration _configuration;

        public RecipesService(
            IRecipeClientService recipeClientService,
            IRepositoryManager repositoryService,
            IDateTimeService dateTimeService,
            ILogger<RecipesService> logger,
            IConfiguration configuration
            )
        {
            _recipeClientService = recipeClientService;
            _repositoryManager = repositoryService;
            _dateTimeService = dateTimeService;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Recipe> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var internalRecipe = await _repositoryManager.RecipeRepository.GetByIdAsync(id, cancellationToken);
            var dataUpdateThreshold = _configuration.GetSection("DataUpdateThresholdInMinutes").Get<double>();

            if (internalRecipe is not null && internalRecipe?.UpdatedAtUtc >= _dateTimeService.UtcNow().AddMinutes(-dataUpdateThreshold))
                return internalRecipe;

            var externalRecipe = await _recipeClientService.GetRecipeAsync(id, cancellationToken);

            if (externalRecipe is null)
            {
                if (internalRecipe is null)
                    throw new ResourceNotFoundException("Failed to fetch recipe from external and internal services");

                _logger.LogWarning($"Failed to fetch recipe from external service, returning outdated internal data");
                return internalRecipe;
            }

            if (internalRecipe is null)
            {
                externalRecipe.UpdatedAtUtc = _dateTimeService.UtcNow();
                await _repositoryManager.RecipeRepository.InsertAsync(externalRecipe, cancellationToken);
            }
            else
            {
                internalRecipe = externalRecipe;
                internalRecipe.UpdatedAtUtc = _dateTimeService.UtcNow();
                _repositoryManager.RecipeRepository.MarkEntityAsModified(internalRecipe);
            }

            await _repositoryManager.SaveChangesAsync(cancellationToken);
            var recipeWithFavoriteState = await _repositoryManager.RecipeRepository.GetByIdAsync(id, cancellationToken);
            return recipeWithFavoriteState;
        }

        public async Task<RecipesPaginatedList> GetPaginatedListAsync(PaginatedListArgs listArgs, CancellationToken cancellationToken)
        {
            var paginatedRecipeData = await _repositoryManager.PaginatedDataCacheRepository.GetPaginatedResultAsync(listArgs, nameof(Recipe), cancellationToken);
            var recipesIds = paginatedRecipeData?.ResourceIds?.Any() == true ? paginatedRecipeData.ResourceIds.Split(",").Select(int.Parse) : Array.Empty<int>();
            var recipes = paginatedRecipeData != null ?
                await _repositoryManager.RecipeRepository.GetByIdsAsync(recipesIds, cancellationToken) : Array.Empty<Recipe>();
            var dataUpdateThreshold = _configuration.GetSection("DataUpdateThresholdInMinutes").Get<double>();
            var outdatedRecipes = recipes?.Where(r => r.UpdatedAtUtc < _dateTimeService.UtcNow().AddMinutes(-dataUpdateThreshold));

            if (paginatedRecipeData is not null && outdatedRecipes?.Any() == false && !IsOutdatedAndEmpty(paginatedRecipeData, dataUpdateThreshold))
                    return new() { Recipes = recipes, PageNumber = listArgs.PageNumber, PageSize = listArgs.PageSize, Total = paginatedRecipeData.Total };
                
            var externalRecipes = await _recipeClientService.GetRecipesAsync(listArgs, cancellationToken);

            if (externalRecipes is null)
            {
                if (paginatedRecipeData is null)
                    throw new InternalServerErrorException("Failed to fetch recipes from external and internal services");

                _logger.LogWarning("Failed to fetch recipes from external service, returning outdated internal data");
                return new() { Recipes = recipes, PageNumber = listArgs.PageNumber, PageSize = listArgs.PageSize, Total = paginatedRecipeData.Total };
            }

            recipesIds = externalRecipes.Recipes.Select(r => r.Id).ToList();
            paginatedRecipeData = CreatePaginatedCacheData(listArgs, recipesIds, externalRecipes);

            externalRecipes.Recipes.ToList().ForEach(r => r.UpdatedAtUtc = _dateTimeService.UtcNow());
            await _repositoryManager.PaginatedDataCacheRepository.InsertOrUpdateAsync(paginatedRecipeData, cancellationToken);
            await _repositoryManager.RecipeRepository.InsertOrUpdateRangeAsync(externalRecipes.Recipes, cancellationToken);
            await _repositoryManager.SaveChangesAsync(cancellationToken);
            recipes = await _repositoryManager.RecipeRepository.GetByIdsAsync(recipesIds, cancellationToken);

            return new() { Recipes = recipes, PageNumber = listArgs.PageNumber, PageSize = listArgs.PageSize, Total = paginatedRecipeData.Total };
        }

        private bool IsOutdatedAndEmpty(PaginatedDataCache paginatedRecipeData, double dataUpdateThreshold)
        {
            return !paginatedRecipeData.ResourceIds.Any() && paginatedRecipeData.UpdatedAtUtc <= _dateTimeService.UtcNow().AddMinutes(-dataUpdateThreshold);
        }

        private PaginatedDataCache CreatePaginatedCacheData(PaginatedListArgs listArgs, IEnumerable<int> recipesIds, RecipesPaginatedList externalRecipes)
        {
            return new PaginatedDataCache
            {
                ResourceName = nameof(Recipe),
                PageNumber = listArgs.PageNumber,
                ItemsPerPage = listArgs.PageSize,
                Total = externalRecipes.Total,
                ResourceIds = string.Join(",", recipesIds),
                UpdatedAtUtc = _dateTimeService.UtcNow(),
            };
        }
    }
}

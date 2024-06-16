using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;
using RecipeAPI.Service.Contract;
using RestSharp;

namespace RecipeAPI.Services
{
    public class RecipesClientService : IRecipeClientService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RecipesClientService> _logger;
        private readonly ExternalDataSourceConfig _config;

        public RecipesClientService(IConfiguration configuration, ILogger<RecipesClientService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Recipe?> GetRecipeAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var config = _configuration.GetSection("ExternalDataSourceConfiguration").Get<ExternalDataSourceConfig>();
                var restClient = new RestClient(config.BaseUrl);
                var request = new RestRequest($"{config.RecipesResourceName}/{id}");
                var restResult = await restClient.ExecuteAsync(request, cancellationToken);

                if (!restResult.IsSuccessful || string.IsNullOrWhiteSpace(restResult.Content))
                {
                    _logger.LogError($"Failed to retrieve data from provider: {restResult.Content}");
                    return null;
                }
                    
                var result = JsonConvert.DeserializeObject<Recipe>(restResult.Content);
                return result;
            }
            catch (Exception ex)            
            {
                _logger.LogError(ex, "Failed to retrieve data from provider");
                return null;
            }
        }

        public async Task<RecipesPaginatedList?> GetRecipesAsync(PaginatedListArgs listArgs, CancellationToken cancellationToken)
        {
            try
            {
                var config = _configuration.GetSection("ExternalDataSourceConfiguration").Get<ExternalDataSourceConfig>();
                var restClient = new RestClient(config.BaseUrl);
                var request = new RestRequest(config.RecipesResourceName);
                request.AddQueryParameter("limit", listArgs.PageSize);
                request.AddQueryParameter("skip", listArgs.PageSize * (listArgs.PageNumber - 1));

                var restResult = await restClient.ExecuteAsync(request, cancellationToken);

                if (!restResult.IsSuccessful || string.IsNullOrWhiteSpace(restResult.Content))
                {
                    _logger.LogError($"Failed to retrieve data from provider: {restResult.Content}");
                    return null;
                }

                var result = JsonConvert.DeserializeObject<RecipesPaginatedList>(restResult.Content);
                return result;
            }
            catch (Exception ex)            
            {
                _logger.LogError(ex, "Failed to retrieve data from provider");
                return null;
            }
        }
    }
}

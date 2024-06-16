using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeAPI.MappingProfiles;
using RecipeAPI.Repository;
using RecipeAPI.Repository.Contract;
using RecipeAPI.Service;
using RecipeAPI.Service.Contract;
using RecipeAPI.Services;

namespace RecipeAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRecipesService, RecipesService>();
            services.AddScoped<IFavoriteRecipesService, FavoriteRecipesService>();
            services.AddScoped<IRecipeClientService, RecipesClientService>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new RecipeMappingProfile());
                config.AddProfile(new ArgsMappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void SetDbContextByCurrentSetting(this IServiceCollection services, ConfigurationManager configuration)
        {
            if (configuration.GetValue<bool>("UseSqlite"))
            {
                services.AddDbContext<RepositoryDbContext>(options =>
                    options.UseSqlite(configuration.GetConnectionString("Sqlite")));
                return;
            }

            services.AddDbContext<RepositoryDbContext>(options =>
                options.UseInMemoryDatabase(configuration.GetConnectionString("InMemory"))
            );

        }
    }
}

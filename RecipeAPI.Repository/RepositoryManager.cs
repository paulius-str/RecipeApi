using RecipeAPI.Repository.Contract;

namespace RecipeAPI.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryDbContext _dbContext;
        private readonly Lazy<IRecipeRepository> _recipeRepository;
        private readonly Lazy<IFavoriteRecipeRepository> _favoriteRecipeRepository;
        private readonly Lazy<IPaginatedDataCacheRepository> _paginatedDataCacheRepository;

        public IRecipeRepository RecipeRepository => _recipeRepository.Value;
        public IFavoriteRecipeRepository FavoriteRecipeRepository => _favoriteRecipeRepository.Value;
        public IPaginatedDataCacheRepository PaginatedDataCacheRepository => _paginatedDataCacheRepository.Value;

        public RepositoryManager(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
            _recipeRepository = new Lazy<IRecipeRepository>(() => new RecipeRepository(dbContext));
            _favoriteRecipeRepository = new Lazy<IFavoriteRecipeRepository>(() => new FavoriteRecipeRepository(dbContext));
            _paginatedDataCacheRepository = new Lazy<IPaginatedDataCacheRepository>(() => new PaginatedDataCacheRepository(dbContext));
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

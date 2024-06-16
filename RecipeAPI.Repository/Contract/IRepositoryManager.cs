namespace RecipeAPI.Repository.Contract
{
    public interface IRepositoryManager
    {
        IFavoriteRecipeRepository FavoriteRecipeRepository { get; }
        IRecipeRepository RecipeRepository { get; }
        IPaginatedDataCacheRepository PaginatedDataCacheRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
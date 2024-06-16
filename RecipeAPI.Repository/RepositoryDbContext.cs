using Microsoft.EntityFrameworkCore;
using RecipeAPI.Model.DataModel;
using RecipeAPI.Repository.Configuration;

namespace RecipeAPI.Repository
{
    public class RepositoryDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
        public DbSet<PaginatedDataCache> PaginatedDataCache { get; set; }

        public RepositoryDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RecipeConfiguration());
        }
    }
}

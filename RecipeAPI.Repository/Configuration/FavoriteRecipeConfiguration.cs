using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeAPI.Model.DataModel;

namespace RecipeAPI.Repository.Configuration
{
    public class FavoriteRecipeConfiguration : IEntityTypeConfiguration<FavoriteRecipe>
    {
        public void Configure(EntityTypeBuilder<FavoriteRecipe> builder)
        {
            builder.HasOne(fr => fr.Recipe);
        }
    }
}

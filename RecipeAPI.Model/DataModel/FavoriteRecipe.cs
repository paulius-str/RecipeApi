using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeAPI.Model.DataModel
{
    [Index(nameof(CreatedAtUtc))]
    [Index(nameof(RecipeId), IsUnique = true)]
    public class FavoriteRecipe
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public Recipe Recipe { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public int RecipeId { get; set; }
    }
}

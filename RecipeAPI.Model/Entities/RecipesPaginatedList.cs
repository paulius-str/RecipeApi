using RecipeAPI.Model.DataModel;

namespace RecipeAPI.Model.Entities
{
    public class RecipesPaginatedList : PaginatedList
    {
        public IEnumerable<Recipe> Recipes { get; set; }
    }
}

namespace RecipeAPI.Shared.DTOs
{
    public class RecipesPaginatedListDto
    {
        public IEnumerable<RecipeDto> Recipes { get; set; }
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

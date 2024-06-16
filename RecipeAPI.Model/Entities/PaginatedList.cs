namespace RecipeAPI.Model.Entities
{
    public class PaginatedList
    {
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

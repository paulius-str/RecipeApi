namespace RecipeAPI.Shared.DTOs
{
    public record PaginatedListArgsDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
    }
}

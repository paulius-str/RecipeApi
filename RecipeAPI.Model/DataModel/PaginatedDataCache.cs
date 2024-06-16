using Microsoft.EntityFrameworkCore;

namespace RecipeAPI.Model.DataModel
{
    [Index(new string[] { nameof(ResourceName), nameof(PageNumber), nameof(ItemsPerPage) })]
    public class PaginatedDataCache
    {
        public int Id { get; set; }
        public string ResourceName { get; set; }
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
        public int Total { get; set; }
        public string ResourceIds { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }
}

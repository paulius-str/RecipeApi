using RecipeAPI.Shared.DTOs;

namespace RecipeAPI.Service.Extensions
{
    public static class ValidationExtensions
    {
        public static void Validate(this PaginatedListArgsDto args)
        {
            if (args.PageNumber < 1)
                throw new BadHttpRequestException("pageNumber parameter must be greater than 0");
            else if (args.PageSize < 1)
                throw new BadHttpRequestException("pageSize parameters must be greater than 0");
        }
    }
}

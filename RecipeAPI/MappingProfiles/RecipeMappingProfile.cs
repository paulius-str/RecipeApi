using AutoMapper;
using RecipeAPI.Model.DataModel;
using RecipeAPI.Model.Entities;
using RecipeAPI.Shared.DTOs;

namespace RecipeAPI.MappingProfiles
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile() 
        { 
            CreateMap<Recipe, RecipeDto>();
            CreateMap<RecipesPaginatedList, RecipesPaginatedListDto>();
        }
    }
}

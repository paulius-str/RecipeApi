using AutoMapper;
using RecipeAPI.Model.Entities;
using RecipeAPI.Shared.DTOs;

namespace RecipeAPI.MappingProfiles
{
    public class ArgsMappingProfile : Profile
    {
        public ArgsMappingProfile() 
        { 
            CreateMap<PaginatedListArgsDto, PaginatedListArgs>();
        }
    }
}

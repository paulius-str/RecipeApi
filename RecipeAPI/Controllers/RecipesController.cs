using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.Model.Entities;
using RecipeAPI.Model.Exceptions;
using RecipeAPI.Service.Contract;
using RecipeAPI.Service.Extensions;
using RecipeAPI.Shared.DTOs;
using System.Net;

namespace RecipeAPI.Controllers
{
    [ApiController]
    [Route("recipes")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipesService _recipesService;
        private readonly IMapper _mapper;

        public RecipesController(IRecipesService recipesService, IMapper mapper)
        {
            _recipesService = recipesService;
            _mapper = mapper;
        }

        [HttpGet("{recipeId}")]
        public async Task<ServerResultWithData<RecipeDto>> GetById(int? recipeId, CancellationToken cancellationToken)
        {
            if (recipeId == null)
                throw new BadRequestException("recipeId query parameter is mandatory");

            var result = await _recipesService.GetByIdAsync(recipeId.Value, cancellationToken);
            return new() { StatusCode = (int)HttpStatusCode.OK, Data = _mapper.Map<RecipeDto>(result) };
        }

        [HttpGet]
        public async Task<ServerResultWithData<RecipesPaginatedListDto>> GetRecipes([FromQuery] PaginatedListArgsDto listArgs, CancellationToken cancellationToken)
        {
            listArgs.Validate();
            var result = await _recipesService.GetPaginatedListAsync(_mapper.Map<PaginatedListArgs>(listArgs), cancellationToken);
            return new() { StatusCode = (int)HttpStatusCode.OK, Data = _mapper.Map<RecipesPaginatedListDto>(result) };
        }
    }
}

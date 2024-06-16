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
    [Route("recipes/favorites")]
    public class FavoriteRecipesController : ControllerBase
    {
        private readonly IFavoriteRecipesService _favoriteRecipesService;
        private readonly IMapper _mapper;

        public FavoriteRecipesController(IFavoriteRecipesService favoriteRecipesService, IMapper mapper)
        {
            _favoriteRecipesService = favoriteRecipesService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ServerResultWithData<RecipesPaginatedListDto>> GetFavorites([FromQuery] PaginatedListArgsDto listArgs, CancellationToken cancellationToken)
        {
            listArgs.Validate();
            var result = await _favoriteRecipesService.GetPaginatedListAsync(_mapper.Map<PaginatedListArgs>(listArgs), cancellationToken);
            return new() { StatusCode = (int)HttpStatusCode.OK, Data = _mapper.Map<RecipesPaginatedListDto>(result) };
        }

        [HttpPost]
        public async Task<ServerResult> SetAsFavorite([FromQuery] int? recipeId, CancellationToken cancellationToken)
        {
            if(recipeId == null)
                throw new BadRequestException("recipeId query parameter is mandatory");

            await _favoriteRecipesService.AddAsync(recipeId.Value, cancellationToken);
            return new() { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpDelete]
        public async Task<ServerResult> RemoveFavorite([FromQuery] int? recipeId, CancellationToken cancellationToken)
        {
            if (recipeId == null)
                throw new BadRequestException("recipeId query parameter is mandatory");

            await _favoriteRecipesService.RemoveAsync(recipeId.Value, cancellationToken);
            return new() { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}

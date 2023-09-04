using AutoMapper;
using Meal_Planner_Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeController(IMapper mapper, IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [HttpGet]
        public IActionResult GetRecipes() 
        {
            var recipes = _recipeRepository.GetRecipes();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            return Ok(recipes);
        }

    }
}

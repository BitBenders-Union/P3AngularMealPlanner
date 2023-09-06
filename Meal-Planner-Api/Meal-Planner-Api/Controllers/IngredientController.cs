using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private IMapper _mapper;
        private IIngredientRepository _ingredientRepository;

        public IngredientController(IMapper mapper, IIngredientRepository ingredientRepository)
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
        }

        // get all Ingredients
        [HttpGet]
        public IActionResult Get()
        {
            var ingredients = _mapper.Map<List<IngredientDTO>>(_ingredientRepository.GetIngredients());

            if (ingredients == null)
                return NotFound("Not Found");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ingredients);
        }

        // get ingredients by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_ingredientRepository.IngredientExists(id))
                return NotFound("Not Found");

            var ingredient = _mapper.Map<IngredientDTO>(_ingredientRepository.GetIngredient(id));
        
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ingredient);

        }

        // Get ingredient by name
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (!_ingredientRepository.IngredientExists(name))
                return NotFound("Not Found");

            var ingredient = _mapper.Map<IngredientDTO>(_ingredientRepository.GetIngredient(name));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ingredient);

        }

        // Get Ingredient By recipeId
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            var ingredient = _mapper.Map<IngredientDTO>(_ingredientRepository.GetIngredientsFromRecipe(recipeId));

            if (ingredient == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ingredient);
        }






    }
}

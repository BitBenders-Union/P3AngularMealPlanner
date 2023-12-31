﻿
namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IAmountRepository _amountRepository;
        private readonly IUnitRepository _unitRepository;

        public IngredientController(IMapper mapper, IIngredientRepository ingredientRepository, IAmountRepository amountRepository, IUnitRepository unitRepository)
        {
            _mapper = mapper;
            _ingredientRepository = ingredientRepository;
            _amountRepository = amountRepository;
            _unitRepository = unitRepository;
        }

        // get all Ingredients
        [HttpGet]
        public IActionResult Get()
        {
            var ingredients = _mapper.Map<List<IngredientDTO>>(_ingredientRepository.GetIngredients());

            if (ingredients == null || ingredients.Count() == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ingredients);
        }

        // get ingredients by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_ingredientRepository.IngredientExists(id))
                return NotFound();

            var ingredient = _mapper.Map<IngredientDTO>(_ingredientRepository.GetIngredient(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ingredient);

        }

        // Get ingredient by name
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (!_ingredientRepository.IngredientExists(name))
                return NotFound();

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
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ingredient);
        }



        [HttpPost]
        public IActionResult CreateIngredient([FromBody] IngredientDTO ingredientCreate)
        {

            // initial Checks
            if (ingredientCreate == null)
                return BadRequest();



            var ingredient = _ingredientRepository.GetIngredient(ingredientCreate.Name);

            if (ingredient != null)
            {
                ModelState.AddModelError("", "Ingredient Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var ingredientMap = _mapper.Map<Ingredient>(ingredientCreate);

            if (!_ingredientRepository.CreateIngredient(ingredientMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


    }
}

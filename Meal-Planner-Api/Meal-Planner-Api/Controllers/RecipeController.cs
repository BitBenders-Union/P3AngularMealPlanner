using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPreparationTimeRepository _preparationTimeRepository;
        private readonly ICookingTimeRepository _cookingTimeRepository;
        private readonly IServingsRepository _servingsRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IInstructionRepository _instructionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAmountRepository _amountRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RecipeController
            (IMapper mapper,
            IRecipeRepository recipeRepository,
            ICategoryRepository categoryRepository,
            IPreparationTimeRepository preparationTimeRepository,
            ICookingTimeRepository cookingTimeRepository,
            IServingsRepository servingsRepository,
            IRatingRepository ratingRepository,
            IIngredientRepository ingredientRepository,
            IInstructionRepository instructionRepository,
            IUserRepository userRepository,
            IAmountRepository amountRepository,
            IUnitRepository unitRepository,
            DataContext context
            )
        {
            _recipeRepository = recipeRepository;
            _categoryRepository = categoryRepository;
            _preparationTimeRepository = preparationTimeRepository;
            _cookingTimeRepository = cookingTimeRepository;
            _servingsRepository = servingsRepository;
            _ratingRepository = ratingRepository;
            _ingredientRepository = ingredientRepository;
            _instructionRepository = instructionRepository;
            _userRepository = userRepository;
            _amountRepository = amountRepository;
            _unitRepository = unitRepository;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRecipes()
        {
            var recipes = _recipeRepository.GetRecipes();

            if (recipes == null || recipes.Count == 0)
                return NotFound();

            var recipesDTO = new List<RecipeDTO>();

            foreach (var recipe in recipes)
            {
                recipesDTO.Add(RecipeToRecipeDTO(recipe));
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipesDTO);
        }

        [HttpGet("ById/{recipeId}")]
        public IActionResult GetRecipe(int recipeId)
        {
            var recipe = _recipeRepository.GetRecipe(recipeId);

            if (recipe == null)
                return NotFound();

            var recipeDTO = RecipeToRecipeDTO(recipe);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipeDTO);
        }

        [HttpGet("ByName/{recipeName}")]
        public IActionResult GetRecipe(string recipeName)
        {
            var recipe = _recipeRepository.GetRecipe(recipeName);

            if (recipe == null)
                return NotFound();

            RecipeDTO recipeDTO = RecipeToRecipeDTO(recipe);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipeDTO);
        }

       

        [HttpGet("{recipeId}/rating")]
        public IActionResult GetRating(int recipeId)
        {
            // check if recipe exist
            if (!_recipeRepository.RecipeExists(recipeId))
                return NotFound();

            // get the rating
            var rating = _recipeRepository.GetRecipeRating(recipeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }

        // get recipe by userid
        [HttpGet("byUserId/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var recipes = (_recipeRepository.GetUserRecipes(userId));

            if (recipes == null)
                return NotFound();

            var recipesDTO = new List<RecipeDTO>();

            foreach (var recipe in recipes)
            {
                recipesDTO.Add(RecipeToRecipeDTO(recipe));
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipesDTO);
        }


        // this also checks if all the necessary data exist, creates them if it doesn't and binds them to the recipe

        [HttpPost("create")]
        public IActionResult CreateRecipe([FromBody] RecipeDTO recipeData)
        {
            if(recipeData == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recipe = _recipeRepository.GetRecipes()
            .FirstOrDefault(x => x.Title.Trim().ToUpper() == recipeData.Title.Trim().ToUpper());

            if (recipe != null)
            {
                ModelState.AddModelError("", "Recipe Already Exists");
                return StatusCode(422, ModelState);
            }

            var newRecipe = new Recipe()
            {
                Title = recipeData.Title,
                Description = recipeData.Description,
                Instructions = _mapper.Map<List<Instruction>>(recipeData.Instructions),
                User = _userRepository.GetUser(recipeData.User.Username)
        };

            // validate all data

           // if category doesn't exist, create it
           var categoryExist = _categoryRepository.CategoriesExists(recipeData.Category.CategoryName);
            if (!categoryExist)
            {
                var category = _mapper.Map<Category>(recipeData.Category);
                _categoryRepository.CreateCategory(category);
                newRecipe.Category = _categoryRepository.GetCategoryFromName(recipeData.Category.CategoryName);
            }
            else
            {
                newRecipe.Category = _categoryRepository.GetCategoryFromName(recipeData.Category.CategoryName);
            }

            // validate preparationTime
            var prepTimeExist = _preparationTimeRepository.PreparationTimeExists(recipeData.PreparationTime);
            if (!prepTimeExist)
            {
                var prep = _mapper.Map<PreparationTime>(recipeData.PreparationTime);
                _preparationTimeRepository.CreatePreparationTime(prep);
                newRecipe.PreparationTime = _preparationTimeRepository.GetPreparationTimeFromMinutes(recipeData.PreparationTime.Minutes);
            }
            else
            {
                newRecipe.PreparationTime = _preparationTimeRepository.GetPreparationTimeFromMinutes(recipeData.PreparationTime.Minutes);
            }


            // validate cookingTime

            var cookTimeExist = _cookingTimeRepository.CookingTimeExists(recipeData.CookingTime);
            if (!cookTimeExist)
            {
                var cook = _mapper.Map<CookingTime>(recipeData.CookingTime);
                _cookingTimeRepository.CreateCookingTime(cook);
                newRecipe.CookingTime = _cookingTimeRepository.GetCookingTimeFromMinutes(recipeData.CookingTime.Minutes);
            }
            else
            {
                newRecipe.CookingTime = _cookingTimeRepository.GetCookingTimeFromMinutes(recipeData.CookingTime.Minutes);
            }

            // validate servings

            var servingExist = _servingsRepository.servingExist(recipeData.Servings);
            if (!servingExist)
            {
                var serv = _mapper.Map<Servings>(recipeData.Servings);
                _servingsRepository.CreateServing(serv);
                newRecipe.Servings = _servingsRepository.GetServingsFromQuantity(recipeData.Servings.Quantity);
            }
            else
            {
                newRecipe.Servings = _servingsRepository.GetServingsFromQuantity(recipeData.Servings.Quantity);
            }



            if (!_recipeRepository.CreateRecipeTest(newRecipe))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            foreach (var ingredientDTO in recipeData.Ingredients)
            {

                var existingIngredient = _ingredientRepository.GetIngredient(ingredientDTO.Name);

                if (existingIngredient == null)
                {
                    existingIngredient = _mapper.Map<Ingredient>(ingredientDTO);
                    _ingredientRepository.CreateIngredient(existingIngredient);
                    existingIngredient = _ingredientRepository.GetIngredient(ingredientDTO.Name);
                }


                var existingAmount = _amountRepository.GetAmountByQuantity(ingredientDTO.Amount.Quantity);

                if (existingAmount == null)
                {
                    existingAmount = _mapper.Map<Amount>(ingredientDTO.Amount);
                    _amountRepository.CreateAmount(existingAmount);
                    existingAmount = _amountRepository.GetAmountByQuantity(ingredientDTO.Amount.Quantity);

                }


                var existingUnit = _unitRepository.GetUnitByName(ingredientDTO.Unit.Measurement);

                if (existingUnit == null)
                {
                    existingUnit = _mapper.Map<Unit>(ingredientDTO.Unit);
                    _unitRepository.CreateUnit(existingUnit);
                    existingUnit = _unitRepository.GetUnitByName(ingredientDTO.Unit.Measurement);

                }


                var recipeIngredient = new RecipeIngredient
                {
                    Recipe = _recipeRepository.GetRecipe(newRecipe.Title),
                    Ingredient = existingIngredient,
                    Amount = existingAmount,
                    Unit = existingUnit,
                };

                newRecipe.RecipeIngredients.Add(recipeIngredient);

            }

            if (!_recipeRepository.UpdateRecipe(newRecipe))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateRecipe(int id, [FromBody] RecipeDTO recipeData)
        {
            if (recipeData == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingRecipe = _recipeRepository.GetRecipe(id);

            if (existingRecipe == null)
            {
                ModelState.AddModelError("", "Recipe not found");
                return NotFound(ModelState);
            }

            // Update properties based on recipeData
            existingRecipe.Title = recipeData.Title;
            existingRecipe.Description = recipeData.Description;

            // Update other properties as needed, e.g., Instructions, User, etc.

            // Validate and update the category
            var categoryExist = _categoryRepository.CategoriesExists(recipeData.Category.CategoryName);
            if (!categoryExist)
            {
                var category = _mapper.Map<Category>(recipeData.Category);
                _categoryRepository.CreateCategory(category);
                existingRecipe.Category = _categoryRepository.GetCategoryFromName(recipeData.Category.CategoryName);
            }
            else
            {
                existingRecipe.Category = _categoryRepository.GetCategoryFromName(recipeData.Category.CategoryName);
            }

            // Validate and update preparationTime
            var prepTimeExist = _preparationTimeRepository.PreparationTimeExists(recipeData.PreparationTime);
            if (!prepTimeExist)
            {
                var prep = _mapper.Map<PreparationTime>(recipeData.PreparationTime);
                _preparationTimeRepository.CreatePreparationTime(prep);
                existingRecipe.PreparationTime = _preparationTimeRepository.GetPreparationTimeFromMinutes(recipeData.PreparationTime.Minutes);
            }
            else
            {
                existingRecipe.PreparationTime = _preparationTimeRepository.GetPreparationTimeFromMinutes(recipeData.PreparationTime.Minutes);
            }

            // Validate and update cookingTime
            var cookTimeExist = _cookingTimeRepository.CookingTimeExists(recipeData.CookingTime);
            if (!cookTimeExist)
            {
                var cook = _mapper.Map<CookingTime>(recipeData.CookingTime);
                _cookingTimeRepository.CreateCookingTime(cook);
                existingRecipe.CookingTime = _cookingTimeRepository.GetCookingTimeFromMinutes(recipeData.CookingTime.Minutes);
            }
            else
            {
                existingRecipe.CookingTime = _cookingTimeRepository.GetCookingTimeFromMinutes(recipeData.CookingTime.Minutes);
            }

            // Validate and update servings
            var servingExist = _servingsRepository.servingExist(recipeData.Servings);
            if (!servingExist)
            {
                var serv = _mapper.Map<Servings>(recipeData.Servings);
                _servingsRepository.CreateServing(serv);
                existingRecipe.Servings = _servingsRepository.GetServingsFromQuantity(recipeData.Servings.Quantity);
            }
            else
            {
                existingRecipe.Servings = _servingsRepository.GetServingsFromQuantity(recipeData.Servings.Quantity);
            }

            // Update the ingredients
            var uniqueIngredients = new HashSet<string>(); // To store unique ingredient names

            existingRecipe.RecipeIngredients.Clear();
            foreach (var ingredientDTO in recipeData.Ingredients)
            {
                if (uniqueIngredients.Contains(ingredientDTO.Name))
                {
                    ModelState.AddModelError("", "Ingredient names must be unique");
                    return BadRequest(ModelState);
                }

                uniqueIngredients.Add(ingredientDTO.Name);

                var existingIngredient = _ingredientRepository.GetIngredient(ingredientDTO.Name);

                if (existingIngredient == null)
                {
                    existingIngredient = _mapper.Map<Ingredient>(ingredientDTO);
                    _ingredientRepository.CreateIngredient(existingIngredient);
                    existingIngredient = _ingredientRepository.GetIngredient(ingredientDTO.Name);
                }

                var existingAmount = _amountRepository.GetAmountByQuantity(ingredientDTO.Amount.Quantity);

                if (existingAmount == null)
                {
                    existingAmount = _mapper.Map<Amount>(ingredientDTO.Amount);
                    _amountRepository.CreateAmount(existingAmount);
                    existingAmount = _amountRepository.GetAmountByQuantity(ingredientDTO.Amount.Quantity);
                }

                var existingUnit = _unitRepository.GetUnitByName(ingredientDTO.Unit.Measurement);

                if (existingUnit == null)
                {
                    existingUnit = _mapper.Map<Unit>(ingredientDTO.Unit);
                    _unitRepository.CreateUnit(existingUnit);
                    existingUnit = _unitRepository.GetUnitByName(ingredientDTO.Unit.Measurement);
                }

                var recipeIngredient = new RecipeIngredient
                {
                    Recipe = existingRecipe,
                    Ingredient = existingIngredient,
                    Amount = existingAmount,
                    Unit = existingUnit,
                };

                existingRecipe.RecipeIngredients.Add(recipeIngredient);
            }

            if (!_recipeRepository.UpdateRecipe(existingRecipe))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


        // delete recipe

        [HttpDelete("delete/{recipeId}")]
        public IActionResult DeleteRecipe(int recipeId)
        {
            if (!_recipeRepository.RecipeExists(recipeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            // when deleting a recipe all the relationships should be deleted as well
            // Only the relationships eg. junction table data is deleted the rest is maintained.
            // but since instructions is unique to the recipe (one recipe to many instructions), the instructions are deleted.

            // in turn if we deleted a user, all the recipes that user created would be deleted using cascade delete,
            // to prevent that we can define the relationship to not delete in the data context

            var recipe = _recipeRepository.GetRecipe(recipeId);

            if (!_recipeRepository.DeleteRecipe(recipe))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }


            return Ok();
        }

        private RecipeDTO RecipeToRecipeDTO(Recipe recipe)
        {
            var recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Category = _mapper.Map<CategoryDTO>(recipe.Category),
                PreparationTime = _mapper.Map<PreparationTimeDTO>(recipe.PreparationTime),
                CookingTime = _mapper.Map<CookingTimeDTO>(recipe.CookingTime),
                Servings = _mapper.Map<ServingsDTO>(recipe.Servings),
                Ingredients = new List<IngredientDTO>(),
                Instructions = _mapper.Map<List<InstructionDTO>>(recipe.Instructions),
                User = _mapper.Map<UserOnlyNameDTO>(recipe.User)
            };
            foreach (var recipeIngredient in recipe.RecipeIngredients)
            {
                IngredientDTO storedIngredient = new()
                {
                    Name = recipeIngredient.Ingredient.Name,
                    Amount = new()
                    {
                        Quantity = recipeIngredient.Amount.Quantity
                    },
                    Unit = new()
                    {
                        Measurement = recipeIngredient.Unit.Measurement
                    }
                };
                recipeDTO.Ingredients.Add(storedIngredient);
            }

            return recipeDTO;
        }



    }
}
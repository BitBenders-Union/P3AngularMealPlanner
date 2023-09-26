using AutoMapper;
using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Meal_Planner_Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private IRecipeRepository _recipeRepository;
        private ICategoryRepository _categoryRepository;
        private IPreparationTimeRepository _preparationTimeRepository;
        private ICookingTimeRepository _cookingTimeRepository;
        private IServingsRepository _servingsRepository;
        private IRatingRepository _ratingRepository;
        private IIngredientRepository _ingredientRepository;
        private IInstructionRepository _instructionRepository;
        private IUserRepository _userRepository;
        private IAmountRepository _amountRepository;
        private IUnitRepository _unitRepository;
        private DataContext _context;
        private IMapper _mapper;

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

            if (recipes == null || recipes.Count() == 0)
                return NotFound("Not Found");

            var recipesDTO = new List<RecipeDTO>();

            foreach (var recipe in recipes)
            {
                var recipeDTO = new RecipeDTO
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Category = _mapper.Map<CategoryDTO>(recipe.category),
                    PreparationTimes = _mapper.Map<PreparationTimeDTO>(recipe.PreparationTime),
                    CookingTimes = _mapper.Map<CookingTimeDTO>(recipe.CookingTime),
                    Servings = _mapper.Map<ServingsDTO>(recipe.Servings),
                    Ratings = recipe.RecipeRating.Select(rr => _mapper.Map<RatingDTO>(rr.Rating)).ToList(),
                    Ingredients = recipe.RecipeIngredients.Select(ri => _mapper.Map<IngredientDTO>(ri.Ingredient)).ToList(),
                    Instructions = _mapper.Map<List<InstructionDTO>>(recipe.Instructions),
                    User = _mapper.Map<UserOnlyNameDTO>(recipe.User)
                };

                recipesDTO.Add(recipeDTO);
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
                return NotFound("Not Found");

            var recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Category = _mapper.Map<CategoryDTO>(recipe.category),
                PreparationTimes = _mapper.Map<PreparationTimeDTO>(recipe.PreparationTime),
                CookingTimes = _mapper.Map<CookingTimeDTO>(recipe.CookingTime),
                Servings = _mapper.Map<ServingsDTO>(recipe.Servings),
                Ratings = recipe.RecipeRating.Select(rr => _mapper.Map<RatingDTO>(rr.Rating)).ToList(),
                Ingredients = recipe.RecipeIngredients.Select(ri => _mapper.Map<IngredientDTO>(ri.Ingredient)).ToList(),
                Instructions = _mapper.Map<List<InstructionDTO>>(recipe.Instructions),
                User = _mapper.Map<UserOnlyNameDTO>(recipe.User)
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipeDTO);
        }

        [HttpGet("ByName/{recipeName}")]
        public IActionResult GetRecipe(string recipeName)
        {
            var recipe = _recipeRepository.GetRecipe(recipeName);

            if (recipe == null)
                return NotFound("Not Found");

            var recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Category = _mapper.Map<CategoryDTO>(recipe.category),
                PreparationTimes = _mapper.Map<PreparationTimeDTO>(recipe.PreparationTime),
                CookingTimes = _mapper.Map<CookingTimeDTO>(recipe.CookingTime),
                Servings = _mapper.Map<ServingsDTO>(recipe.Servings),
                Ratings = recipe.RecipeRating.Select(rr => _mapper.Map<RatingDTO>(rr.Rating)).ToList(),
                Ingredients = recipe.RecipeIngredients.Select(ri => _mapper.Map<IngredientDTO>(ri.Ingredient)).ToList(),
                Instructions = _mapper.Map<List<InstructionDTO>>(recipe.Instructions),
                User = _mapper.Map<UserOnlyNameDTO>(recipe.User)
            };

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

        // get recipe a user created
        [HttpGet("byUserId/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var recipe = _mapper.Map<List<RecipeDTO>>(_recipeRepository.GetUserRecipes(userId));

            if (recipe == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipe);
        }


        // this also checks if all the necessary data exist, creates them if it doesn't and binds them to the recipe
        [HttpPost("create")]
        public IActionResult CreateRecipe([FromBody] RecipeDTO recipeData)
        {

            // Get a full recipe from body.

            if (recipeData == null)
                return BadRequest();

            var recipe = _recipeRepository.GetRecipes()
                .FirstOrDefault(x => x.Title.Trim().ToUpper() == recipeData.Title.Trim().ToUpper());

            if (recipe != null)
            {
                ModelState.AddModelError("", "Recipe Already Exists");
                return StatusCode(422, ModelState);
            }

            var recipeMap = _mapper.Map<Recipe>(recipeData);

            // validate all data

            // if category doesn't exist, create it
            var categoryExist = _categoryRepository.CategoriesExists(recipeData.Category.CategoryName);
            if (!categoryExist)
            {
                var category = _mapper.Map<Category>(recipeData.Category);
                _categoryRepository.CreateCategory(category);
                recipeMap.category = _categoryRepository.GetCategoryFromName(recipeData.Category.CategoryName);
            }
            else
            {
                recipeMap.category = _categoryRepository.GetCategoryFromName(recipeData.Category.CategoryName);
            }

            // validate preparationTime
            var prepTimeExist = _preparationTimeRepository.PreparationTimeExists(recipeData.PreparationTimes);
            if (!prepTimeExist)
            {
                var prep = _mapper.Map<PreparationTime>(recipeData.PreparationTimes);
                _preparationTimeRepository.CreatePreparationTime(prep);
                recipeMap.PreparationTime = _preparationTimeRepository.GetPreparationTimeFromMinutes(recipeData.PreparationTimes.Minutes);
            }
            else
            {
                recipeMap.PreparationTime = _preparationTimeRepository.GetPreparationTimeFromMinutes(recipeData.PreparationTimes.Minutes);
            }


            // validate cookingTime

            var cookTimeExist = _cookingTimeRepository.CookingTimeExists(recipeData.CookingTimes);
            if (!cookTimeExist)
            {
                var cook = _mapper.Map<CookingTime>(recipeData.CookingTimes);
                _cookingTimeRepository.CreateCookingTime(cook);
                recipeMap.CookingTime = _cookingTimeRepository.GetCookingTimeFromMinutes(recipeData.CookingTimes.Minutes);
            }
            else
            {
                recipeMap.CookingTime = _cookingTimeRepository.GetCookingTimeFromMinutes(recipeData.CookingTimes.Minutes);
            }

            // validate servings

            var servingExist = _servingsRepository.servingExist(recipeData.Servings);
            if (!servingExist)
            {
                var serv = _mapper.Map<Servings>(recipeData.Servings);
                _servingsRepository.CreateServing(serv);
                recipeMap.Servings = _servingsRepository.GetServingsFromQuantity(recipeData.Servings.Quantity);
            }
            else
            {
                recipeMap.Servings = _servingsRepository.GetServingsFromQuantity(recipeData.Servings.Quantity);
            }

            // validate ratings
            // since the ratings should only be 1 rating on creation
            // we don't loop through but takes the first item in the list
            // and use as the rating
            List<int> ratingIds = new();
            if (!_ratingRepository.ratingExists(recipeData.Ratings))
            {
                var rating = _mapper.Map<Rating>(recipeData.Ratings.FirstOrDefault());
                _ratingRepository.CreateRating(rating);
                ratingIds.Add(_ratingRepository.GetRatingId(recipeData.Ratings.First().Score));
            }
            else
            {
                ratingIds.Add(_ratingRepository.GetRatingId(recipeData.Ratings.First().Score));
            }


            // validate Ingredients
            // since there can be multiple ingredients we need to loop through each
            // Create a list to store the ingredient IDs
            List<int> ingredientIds = new List<int>();

            // Dictionaries to prevent duplicate entries.
            Dictionary<float, Amount> createdAmounts = new Dictionary<float, Amount>();
            Dictionary<string, Unit> createdUnits = new Dictionary<string, Unit>();

            foreach (var ingredient in recipeData.Ingredients)
            {
                var ingredientMap = new Ingredient();

                // Check if the ingredient already exists in the database
                if (_ingredientRepository.IngredientExists(ingredient.Name))
                {
                    // Get the existing ingredient from the database
                    ingredientMap = _ingredientRepository.GetIngredient(ingredient.Name);
                }
                else
                {
                    // Set the name for new ingredients
                    ingredientMap.Name = ingredient.Name;
                }

                // Create a new IngredientAmount relationship
                var ingredientAmount = new IngredientAmount();

                // Create Amount entity
                var amountEntity = new Amount { Quantity = ingredient.Amount.Quantity };

                // Check if the amount with the same quantity already exists in the database
                if (_amountRepository.AmountExistByQuantity(amountEntity.Quantity))
                {
                    // Use the existing amount entity
                    var existingAmount = _amountRepository.GetAmountByQuantity(amountEntity.Quantity);
                    ingredientAmount.amount = existingAmount;
                }
                else if (createdAmounts.TryGetValue(amountEntity.Quantity, out var existingAmountEntity))
                {
                    // Use the existing amount entity from the dictionary
                    ingredientAmount.amount = existingAmountEntity;
                }
                else
                {
                    // Add the amount to the dictionary
                    createdAmounts.Add(amountEntity.Quantity, amountEntity);

                    // Associate the amount with the IngredientAmount relationship
                    ingredientAmount.amount = amountEntity;
                }

                // Add the IngredientAmount relationship to the Ingredient
                ingredientMap.IngredientAmount = new List<IngredientAmount> { ingredientAmount };

                // Create a new IngredientUnit relationship
                var ingredientUnit = new IngredientUnit();

                // Create Unit entity
                var unitEntity = new Unit { Measurement = ingredient.Unit.Measurement };

                // Check if the unit with the same measurement already exists in the database
                if (_unitRepository.UnitExists(unitEntity.Measurement))
                {
                    // Use the existing unit entity
                    var existingUnit = _unitRepository.GetUnitByName(unitEntity.Measurement);
                    ingredientUnit.unit = existingUnit;
                }
                else if (createdUnits.TryGetValue(unitEntity.Measurement, out var existingUnitEntity))
                {
                    // Use the existing unit entity from the dictionary
                    ingredientUnit.unit = existingUnitEntity;
                }
                else
                {
                    // Add the unit to the dictionary
                    createdUnits.Add(unitEntity.Measurement, unitEntity);

                    // Associate the unit with the IngredientUnit relationship
                    ingredientUnit.unit = unitEntity;
                }

                // Add the IngredientUnit relationship to the Ingredient
                ingredientMap.IngredientUnit = new List<IngredientUnit> { ingredientUnit };

                // Check if the ingredient already exists in the database
                if (!_ingredientRepository.IngredientExists(ingredient.Name))
                {
                    // Create the ingredient and add its ID to the list
                    _ingredientRepository.CreateIngredient(ingredientMap);
                }

                // Add the ingredient ID to the list
                ingredientIds.Add(ingredientMap.Id);
            }





            // create the recipe using the id's from the data we just created

            recipeMap.User = _userRepository.GetUser(recipeData.User.Username);

            if (!_recipeRepository.CreateRecipe(recipeMap, ratingIds, ingredientIds))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }



            // validate instructions
            // needs to be after creating the recipe, because of one to many rules

            foreach (var instruction in recipeData.Instructions)
            {
                var instructionMap = _mapper.Map<Instruction>(instruction);
                instructionMap.Recipe = recipeMap;
                _instructionRepository.CreateInstruction(instructionMap);

            }

            return Ok();
        }

        // Update recipe
        [HttpPut("update/{recipeId}")]
        public IActionResult UpdateRecipe([FromBody] RecipeDTO recipeData, int recipeId)
        {
            if(recipeData == null)
                return BadRequest();

            if(!_recipeRepository.RecipeExists(recipeId))
                return NotFound("Recipe with given id, does not exist");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            recipeData.Id = recipeId;
            var ExistingRecipe = _recipeRepository.GetRecipe(recipeId);
            ExistingRecipe.Title = recipeData.Title;
            ExistingRecipe.Description = recipeData.Description;

            // validate all data
            // if data doesn't exist, create it
            // then update recipe with the new data
                
            // category
            var categoryExist = _categoryRepository.CategoriesExists(recipeData.Category.CategoryName);
            if (!categoryExist)
            {
                var category = _mapper.Map<Category>(recipeData.Category);
                _categoryRepository.CreateCategory(category);
                ExistingRecipe.category = _categoryRepository.GetCategoryFromName(recipeData.Category.CategoryName);
            }
            else
            {
                ExistingRecipe.category = _categoryRepository.GetCategoryFromName(recipeData.Category.CategoryName);
            }

            // preparationTime
            var prepTimeExist = _preparationTimeRepository.PreparationTimeExists(recipeData.PreparationTimes);
            if(!prepTimeExist)
            {
                var prepTime = _mapper.Map<PreparationTime>(recipeData.PreparationTimes);
                _preparationTimeRepository.CreatePreparationTime(prepTime);
                ExistingRecipe.PreparationTime = _preparationTimeRepository.GetPreparationTimeFromMinutes(recipeData.PreparationTimes.Minutes);
            }
            else
            {
                ExistingRecipe.PreparationTime = _preparationTimeRepository.GetPreparationTimeFromMinutes(recipeData.PreparationTimes.Minutes);
            }

            // cookingTime

            var cookTimeExist = _cookingTimeRepository.CookingTimeExists(recipeData.CookingTimes);
            if(!cookTimeExist)
            {
                var cookTime = _mapper.Map<CookingTime>(recipeData.CookingTimes);
                _cookingTimeRepository.CreateCookingTime(cookTime);
                ExistingRecipe.CookingTime = _cookingTimeRepository.GetCookingTimeFromMinutes(recipeData.CookingTimes.Minutes);
            }
            else
            {
                ExistingRecipe.CookingTime = _cookingTimeRepository.GetCookingTimeFromMinutes(recipeData.CookingTimes.Minutes);
            }

            // servings
            var servingsExist = _servingsRepository.servingExist(recipeData.Servings);
            if(!servingsExist)
            {
                var servings = _mapper.Map<Servings>(recipeData.Servings);
                _servingsRepository.CreateServing(servings);
                ExistingRecipe.Servings = _servingsRepository.GetServingsFromQuantity(recipeData.Servings.Quantity);
            }
            else
            {
                ExistingRecipe.Servings = _servingsRepository.GetServingsFromQuantity(recipeData.Servings.Quantity);
            }
            

            // ratings
            // only change first rating since we only allow one rating on creation
            
            if(!_ratingRepository.ratingExists(recipeData.Ratings))
            {
                var rating = _mapper.Map<Rating>(recipeData.Ratings.FirstOrDefault());
                _ratingRepository.CreateRating(rating);
                ExistingRecipe.RecipeRating.First().Rating = _ratingRepository.GetRatingFromScore(recipeData.Ratings.First().Score);
            }
            else
            {
                ExistingRecipe.RecipeRating.First().Rating = _ratingRepository.GetRatingFromScore(recipeData.Ratings.First().Score);
            }

            // ingredient handling

            // Dictionaries to prevent duplicate entries.
            Dictionary<float, Amount> createdAmounts = new Dictionary<float, Amount>();
            Dictionary<string, Unit> createdUnits = new Dictionary<string, Unit>();
            List<int> ingredientIds = new List<int>();


            foreach (var ingredient in recipeData.Ingredients)
            {

                var existingIngredient = _ingredientRepository.GetIngredient(ingredient.Name);

                // check if ingredient 

                if (existingIngredient == null)
                {
                    // Create a new ingredient since it doesn't exist
                    existingIngredient = _mapper.Map<Ingredient>(ingredient);
                    _ingredientRepository.CreateIngredient(existingIngredient);
                }
                else
                {
                    // Update existing ingredient properties if needed
                    if (existingIngredient.Name != ingredient.Name)
                    {
                        existingIngredient.Name = ingredient.Name;
                        
                    }
                }

                // check if amount 

                var existingAmount = _amountRepository.GetAmountByQuantity(ingredient.Amount.Quantity);


                if (createdAmounts.TryGetValue(ingredient.Amount.Quantity, out var amountEntity) && existingAmount != null)
                {
                    existingAmount.Quantity = amountEntity.Quantity;
                    existingIngredient.IngredientAmount.First().amount = existingAmount;
                }
                else if(existingAmount == null)
                {
                    // create new amount since it doesn't exist
                    existingAmount = _mapper.Map<Amount>(ingredient.Amount);
                    _amountRepository.CreateAmount(existingAmount);
                    createdAmounts.Add(existingAmount.Quantity, existingAmount);
                    //existingIngredient.IngredientAmount.First().amount = existingAmount;
                }
                else
                {
                    if(existingAmount.Quantity != ingredient.Amount.Quantity)
                    {
                        existingAmount.Quantity = ingredient.Amount.Quantity;
                        existingIngredient.IngredientAmount.First().amount = existingAmount;
                    }
                }




                // check if unit


                var existingUnit = _unitRepository.GetUnitByName(ingredient.Unit.Measurement);


                if (createdUnits.TryGetValue(ingredient.Unit.Measurement, out var unitEntity) && existingUnit != null)
                {
                    // take unit from dictionary
                    existingUnit.Measurement = unitEntity.Measurement;
                    existingIngredient.IngredientUnit.First().unit = existingUnit;
                }
                else if (existingUnit == null)
                {
                    // create new unit since it doesn't exist
                    existingUnit = _mapper.Map<Unit>(ingredient.Unit);
                    _unitRepository.CreateUnit(existingUnit);
                    createdUnits.Add(existingUnit.Measurement, existingUnit);

                    //existingIngredient.IngredientUnit.First().unit = existingUnit;
                }
                else
                {
                    if (existingUnit.Measurement != ingredient.Unit.Measurement)
                    {
                        existingUnit.Measurement = ingredient.Unit.Measurement;
                        existingIngredient.IngredientUnit.First().unit = existingUnit;
                    }
                }




                // Create or update the relationships

                // IngredientAmount
                var ingredientAmount = existingIngredient.IngredientAmount.FirstOrDefault();
                var existingIngredientAmount = existingIngredient.IngredientAmount.FirstOrDefault();

                if (ingredientAmount == null)
                {
                    ingredientAmount = new IngredientAmount
                    {
                        ingredient = existingIngredient,
                        ingredientId = existingIngredient.Id,
                        amount = existingAmount,
                        amountId = existingAmount.Id
                    };
                    existingIngredient.IngredientAmount = existingIngredient.IngredientAmount ?? new List<IngredientAmount>();

                    existingIngredient.IngredientAmount.Add(ingredientAmount);
                }
                else
                {
                    // Remove the existing IngredientAmount
                    _context.IngredientAmounts.Remove(existingIngredientAmount);

                    // Create a new IngredientAmount
                    var newIngredientAmount = new IngredientAmount
                    {
                        ingredient = existingIngredient,
                        ingredientId = existingIngredient.Id,
                        amount = existingAmount,
                        amountId = existingAmount.Id
                    };

                    // Add the new IngredientAmount to the list
                    existingIngredient.IngredientAmount.Add(newIngredientAmount);
                }

                // IngredientUnit
                var ingredientUnit = existingIngredient.IngredientUnit.FirstOrDefault();
                var existingIngredientUnit = existingIngredient.IngredientUnit.FirstOrDefault();

                if (ingredientUnit == null)
                {
                    var newIngredientUnit = new IngredientUnit
                    {
                        ingredient = existingIngredient,
                        ingredientId = existingIngredient.Id,
                        unit = existingUnit,
                        unitId = existingUnit.Id
                    };

                    existingUnit.ingredientUnit = existingUnit.ingredientUnit ?? new List<IngredientUnit>();

                    existingUnit.ingredientUnit.Add(newIngredientUnit);
                }
                else
                {
                    _context.IngredientUnits.Remove(existingIngredientUnit);

                    // create new relationship
                    var newIngredientUnit = new IngredientUnit
                    {
                        ingredient = existingIngredient,
                        ingredientId = existingIngredient.Id,
                        unit = existingUnit,
                        unitId = existingUnit.Id
                    };

                    // add new relationship to list
                    existingIngredient.IngredientUnit.Add(newIngredientUnit);
                }

                var recipeIngredient = ExistingRecipe.RecipeIngredients.FirstOrDefault(ri => ri.IngredientId == existingIngredient.Id);

                if (recipeIngredient == null)
                {
                    // Create a new RecipeIngredient if it doesn't exist
                    recipeIngredient = new RecipeIngredient
                    {
                        Recipe = ExistingRecipe,
                        RecipeId = ExistingRecipe.Id,
                        Ingredient = existingIngredient,
                        IngredientId = existingIngredient.Id
                    };

                    // Add the RecipeIngredient to the Recipe
                    ExistingRecipe.RecipeIngredients.Add(recipeIngredient);
                }
            }

            // identify and create a list of ingredients that should be removed from the ExistingRecipe

            var ingredientsToRemove = ExistingRecipe.RecipeIngredients
                .Where(ri => !recipeData.Ingredients.Any(ingredientDTO => ingredientDTO.Name == ri.Ingredient.Name))
                .ToList();

            foreach (var ingredientToRemove in ingredientsToRemove)
            {
                ExistingRecipe.RecipeIngredients.Remove(ingredientToRemove);
            }

            // instruction handling


            foreach (var existingInstruction in ExistingRecipe.Instructions.ToList())
            {
                var updatedInstruction = recipeData.Instructions.FirstOrDefault(i => i.Text == existingInstruction.Text);

                if (updatedInstruction != null)
                {
                    // Update the existing instruction text
                    existingInstruction.Text = updatedInstruction.Text;
                }
                else
                {
                    // Remove the instruction if it doesn't exist in the updated data
                    ExistingRecipe.Instructions.Remove(existingInstruction);
                    _instructionRepository.DeleteInstruction(existingInstruction); // Delete from the database
                }
            }

            foreach (var newInstruction in recipeData.Instructions)
            {
                var existingInstruction = ExistingRecipe.Instructions.FirstOrDefault(i => i.Text == newInstruction.Text);

                if (existingInstruction == null)
                {
                    // Create and add a new instruction if it doesn't exist in the existing data
                    var instructionMap = _mapper.Map<Instruction>(newInstruction);
                    instructionMap.Recipe = ExistingRecipe;
                    ExistingRecipe.Instructions.Add(instructionMap);
                    _instructionRepository.CreateInstruction(instructionMap);
                }
            }



            if (!_recipeRepository.UpdateRecipe(ExistingRecipe))
            {
                ModelState.AddModelError("" , "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }
        
        
        // delete recipe

        [HttpDelete("delete/{recipeId}")]
        public IActionResult DeleteRecipe(int recipeId)
        {
            if(!_recipeRepository.RecipeExists(recipeId))
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            // when deleting a recipe all the relationships should be deleted as well
            // Only the relationships eg. junctiontable data is deleted the rest is maintained.
            // but since instructions is unique to the recipe (one recipe to many instructions), the instructions are deleted.

            // in turn if we deleted a user, all the recipes that user created would be deleted using cascade delete,
            // to prevent that we can define the relationship to not delete in the datacontext

            var recipe = _recipeRepository.GetRecipe(recipeId);

            if(!_recipeRepository.DeleteRecipe(recipe))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }


            return NoContent();
        }



    }
}
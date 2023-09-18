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
            IUserRepository userRepository
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
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetRecipes()
        {
            var recipes = _mapper.Map<List<RecipeDTO>>(_recipeRepository.GetRecipes());

            if (recipes == null || recipes.Count() == 0)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            return Ok(recipes);
        }

        [HttpGet("ById/{recipeId}")]
        public IActionResult GetRecipe(int recipeId)
        {
            // maps the recipe to recipeDTO so we only show what we want.
            var recipe = _mapper.Map<RecipeDTO>(_recipeRepository.GetRecipe(recipeId));

            if (recipe == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipe);
        }

        [HttpGet("ByName/{recipeName}")]
        public IActionResult GetRecipe(string recipeName)
        {
            // maps the recipe to recipeDTO so we only show what we want.
            var recipe = _mapper.Map<RecipeDTO>(_recipeRepository.GetRecipe(recipeName));

            if (recipe == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipe);
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





        // this should only be called afer the creation of necessary data, cookingtime, preptime, servings, ingredients, rating, instruction, user, category
        [HttpPost("create")]
        public IActionResult CreateRecipe([FromBody] RecipeDTO recipeData, [FromQuery] int categoryId, [FromQuery] int preparationTimeId, [FromQuery] int cookingTimeId, [FromQuery] int ServingsId, [FromQuery] List<int> ratingIds, [FromQuery] List<int> instructionIds, [FromQuery] int userId)
        {

            if (recipeData == null)
                return BadRequest();


            var recipe = _recipeRepository.GetRecipes()
                .FirstOrDefault(x => x.Title.Trim().ToUpper() == recipeData.Title.Trim().ToUpper());


            if (recipe != null)
            {
                ModelState.AddModelError("", "Recipe Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recipeMap = _mapper.Map<Recipe>(recipeData);


            //if (!_recipeRepository.CreateRecipe(recipeMap, recipeData.Ratings, recipeData.Ingredients))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }



        [HttpPost("Test")]
        public IActionResult test([FromBody] RecipeDTO recipeData)
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
            List<int> ingredientIds = new();

            foreach (var ingredient in recipeData.Ingredients)
            {
                if (!_ingredientRepository.IngredientExists(ingredient.Name))
                {
                    var ingredientMap = _mapper.Map<Ingredient>(ingredient);
                    _ingredientRepository.CreateIngredient(ingredientMap);
                    ingredientIds.Add(_ingredientRepository.GetIngredient(ingredient.Name).Id);
                }
                else
                {
                    ingredientIds.Add(_ingredientRepository.GetIngredient(ingredient.Name).Id);
                }

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


    }
}

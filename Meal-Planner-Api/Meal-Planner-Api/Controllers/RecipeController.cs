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
        private IMapper _mapper;

        public RecipeController(IMapper mapper, IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
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

        [HttpGet("{recipeId}")]
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

        [HttpGet("{recipeName}")]
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

            if(recipe == null)
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(recipe);
        }





        [HttpPost("/create")]
        public IActionResult CreateRecipe([FromBody] RecipeDTO recipeData)
        {
            if (recipeData == null)
                return BadRequest();


            var recipe = _recipeRepository.GetRecipes()
                .FirstOrDefault(x => x.Title == recipeData.Title);


            if (recipe != null)
            {
                ModelState.AddModelError("", "Recipe Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recipeMap = _mapper.Map<Recipe>(recipeData);


            if (!_recipeRepository.CreateRecipe(recipeMap, recipeData.RatingIDs, recipeData.IngredientIDs))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }


       


    }
}

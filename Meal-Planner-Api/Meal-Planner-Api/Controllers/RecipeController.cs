using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Meal_Planner_Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAmountRepository _amountRepository;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeRepository recipeRepository, IAmountRepository amountRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _amountRepository = amountRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetAllRecipes()
        {
            List<RecipeDTO> recipes = _recipeRepository.GetAllRecipes()
                .Select(recipe => _mapper.Map<RecipeDTO>(recipe))
                .ToList();
            return Ok(recipes);
        }

        [HttpGet("{id}", Name = "GetRecipe")]
        public ActionResult<RecipeDTO> GetRecipe(int id)
        {
            var recipe = _recipeRepository.GetRecipe(id);
            if (recipe == null)
            {
                return NotFound();
            }
            var recipeDTO = _mapper.Map<RecipeDTO>(recipe);
            return Ok(recipeDTO);
        }

        [HttpPost]
        public IActionResult CreateRecipe([FromBody] Recipe recipe)
        {
            // Add the instructions to the recipe
            recipe.Instructions = recipe.Instructions.Select(instructionText =>
                new Instruction { Text =  instructionText.Text}).ToList();


            // Add the recipe and its associated instructions to the database
            _recipeRepository.AddRecipe(recipe);

            // Return the newly created recipe
            var createdRecipe = _mapper.Map<Recipe>(recipe);
            return CreatedAtAction(nameof(GetRecipe), new { id = createdRecipe.Id }, createdRecipe);
        }


        [HttpPut("{id}")]
        public ActionResult UpdateRecipe(int id, RecipeDTO recipeUpdateDto)
        {
            var recipeModel = _mapper.Map<Recipe>(recipeUpdateDto);
            recipeModel.Id = id; // Make sure to set the ID to the existing recipe's ID
            _recipeRepository.UpdateRecipe(recipeModel);

            return NoContent(); // Returns a 204 No Content response
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            var recipe = _recipeRepository.GetRecipe(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _recipeRepository.DeleteRecipe(id);

            return NoContent();
        }


    }
}

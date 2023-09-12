using AutoMapper;
using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private DataContext _context;
        private IMapper _mapper;

        public RecipeRepository(DataContext context, IMapper map)
        {
            _context = context;
            _mapper = map;
        }


        public Recipe GetRecipe(int id)
        {

            return _context.Recipes.FirstOrDefault(x => x.Id == id); // finds first value in recipe where recipe.id is the sam as input id
        }

        public Recipe GetRecipe(string name)
        {
            return _context.Recipes.FirstOrDefault(x => x.Title == name); // finds first value in recipe where recipe.title is the sam as input name

        }

        public float GetRecipeRating(int recipeId)
        {
            var recipe = _context.Recipes.Include(rr => rr.RecipeRating).FirstOrDefault(r => r.Id == recipeId); // finds the recipe from id

            if (recipe != null)
            {
                // get average rating
                float avgRating = recipe.RecipeRating.Average(rr => rr.Rating.Score);
                return avgRating;
            }
            else return 0.0f; // if recipe doesn't exist return 0.0f as rating
        }

        public ICollection<Recipe> GetRecipes()
        {
            return _context.Recipes.OrderBy(x => x.Id).ToList();
        }

        public ICollection<Recipe> GetUserRecipes(int userId)
        {
            return _context.Recipes.Where(r => r.User.Id == userId).ToList();
        }

        public bool RecipeExists(int recipeId)
        {
            return _context.Recipes.Any(r => r.Id == recipeId);
        }
        public bool CreateRecipe(RecipeCreateDTO recipeData)
        {

            Recipe recipe = new()
            {
                Title = recipeData.Title,
                Description = recipeData.Description,
                category = _context.Categories.Where(x => x.Id == recipeData.categoryId).FirstOrDefault(),
                User = _context.Users.Where(x => x.Id == recipeData.userID).FirstOrDefault(),

            };

            foreach (var instructionDto in recipeData.instructions)
            {
                var instruction = new Instruction()
                {
                    Id = instructionDto.Id,
                    Text = instructionDto.Text,
                    // Set other properties of the Instruction
                };
                _context.Add(instruction);
            }



            var recipePrepEntity = _context.PreparationTimes.Where(x => x.Id == recipeData.preparationTimeID).FirstOrDefault();
            var recipeCookEntity = _context.CookingTimes.Where(x => x.Id == recipeData.cookingTimeID).FirstOrDefault();
            var recipeServEntity = _context.Servings.Where(x => x.Id == recipeData.servingsID).FirstOrDefault();

            foreach (var ratingId in recipeData.ratingsID)
            {
                var existingRating = _context.Ratings.SingleOrDefault(r => r.Id == ratingId);
                if (existingRating != null)
                {
                    var recipeRating = new RecipeRating()
                    {
                        Rating = existingRating,
                        Recipe = recipe
                    };
                    _context.Add(recipeRating);
                }
            }

            foreach (var ingredientId in recipeData.ingredientsID)
            {
                var existingIngredient = _context.Ingredients.SingleOrDefault(i => i.Id == ingredientId);
                if (existingIngredient != null)
                {
                    var recipeIngredient = new RecipeIngredient()
                    {
                        Ingredient = existingIngredient,
                        Recipe = recipe
                    };
                    _context.Add(recipeIngredient);
                }
            }

            var recipePrep = new RecipePreparationTime()
            {
                PreparationTime = recipePrepEntity,
                Recipe = recipe
            };
            _context.Add(recipePrep);

            var recipeCook = new RecipeCookingTime()
            {
                CookingTime = recipeCookEntity,
                Recipe = recipe
            };
            _context.Add(recipeCook);

            var recipeServ = new RecipeServings()
            {
                Servings = recipeServEntity,
                Recipe = recipe
            };
            _context.Add(recipeServ);


            _context.Add(recipe);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

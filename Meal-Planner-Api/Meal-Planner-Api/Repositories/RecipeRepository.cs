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
        public bool CreateRecipe(Recipe recipe, ICollection<int> ratingIds, ICollection<int> ingredientIds)
        {

            foreach (var ratingId in ratingIds) 
            {
                var recipeRating = new RecipeRating
                {
                    Rating = _context.Ratings.FirstOrDefault(r => r.Id == ratingId),
                    Recipe = recipe
                };
                _context.Add(recipeRating);
            }

            foreach (var ingredientId in ingredientIds)
            {
                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = _context.Ingredients.FirstOrDefault(i => i.Id == ingredientId),
                    Recipe = recipe
                };
                _context.Add(recipeIngredient);
            }

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

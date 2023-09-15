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
       

        public bool CreateRecipe(RecipeDTO recipe)
        {


            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == recipe.Category.Id);
            var existingPreparationTime = _context.PreparationTimes.FirstOrDefault(p => p.Id == recipe.PreparationTimes.Id);
            var existingCookingTime = _context.CookingTimes.FirstOrDefault(c => c.Id == recipe.CookingTimes.Id);
            var existingServings = _context.Servings.FirstOrDefault(s => s.Id == recipe.Servings.Id);
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == recipe.UserId);
            // u are here

            if (existingUser.Id == null || existingUser.Username == null || existingUser.PasswordHash == null)
            {
                //existingUser = _context.Users.FirstOrDefault(u => u.Username == recipe.User.Username);
            }

            if (existingCategory == null)
            {
                existingCategory = new Category { CategoryName = recipe.Category.CategoryName };
                _context.Categories.Add(existingCategory);
            }

            if (existingPreparationTime == null)
            {
                existingPreparationTime = new PreparationTime { Minutes = recipe.PreparationTimes.Minutes };
                _context.PreparationTimes.Add(existingPreparationTime);
            }

            if (existingCookingTime == null)
            {
                existingCookingTime = new CookingTime { Minutes = recipe.CookingTimes.Minutes };
                _context.CookingTimes.Add(existingCookingTime);
            }

            if (existingServings == null)
            {
                existingServings = new Servings { Quantity = recipe.Servings.Quantity };
                _context.Servings.Add(existingServings);
            }

            if (existingUser == null)
            {
                Console.WriteLine("User doesn't exist");
                Console.WriteLine(recipe.UserId);
                return false;
            }

            var recipeMap = _mapper.Map<Recipe>(recipe);

            recipeMap.Category = existingCategory;
            recipeMap.PreparationTime = existingPreparationTime;
            recipeMap.CookingTime = existingCookingTime;
            recipeMap.Servings = existingServings;
            recipeMap.User = existingUser;
            Console.WriteLine(recipeMap.User);

            foreach (var rating in recipe.Ratings)
            {
                var recipeRating = new RecipeRating
                {
                    Rating = _mapper.Map<Rating>(rating),
                    Recipe = recipeMap
                };
                _context.RecipeRatings.Add(recipeRating);
            }





            foreach (var ingredientDto in recipe.Ingredients)
            {
                var ingredient = _context.Ingredients.FirstOrDefault(i => i.Id == ingredientDto.Id);

                if (ingredient != null)
                {
                    var recipeIngredient = new RecipeIngredient
                    {
                        Ingredient = ingredient,
                        Recipe = recipeMap,
                        Amount = _mapper.Map<Amount>(ingredientDto.Amount),
                        Unit = _mapper.Map<Unit>(ingredientDto.Unit)
                    };
                    _context.RecipeIngredients.Add(recipeIngredient);
                }
            }





            

            _context.Recipes.Add(recipeMap);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

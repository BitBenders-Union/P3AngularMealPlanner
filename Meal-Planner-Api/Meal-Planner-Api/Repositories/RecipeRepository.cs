using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DataContext _context;

        public RecipeRepository(DataContext context)
        {
            _context = context;
        }

        public Recipe GetRecipe(int id)
        {
            return _context.Recipes
                            .Include(r => r.Ingredients)
                                .ThenInclude(i => i.Amounts)
                            .Include(r => r.Instructions)
                            .SingleOrDefault(r => r.Id == id);
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            return _context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.Amounts) // Include Amounts within Ingredients
                .Include(r => r.Instructions)
                .ToList();
        }

        public Recipe AddRecipe(Recipe recipe) { 
            _context.Recipes.Add(recipe);
            _context.SaveChanges();
            return recipe;
        }

        // upsert
        public void UpdateRecipe(Recipe recipe)
        {

            // existingRecipe is a reference to _context and therefore changes to existingRecipe will reflect _context.
            // then we include ingredients and instructions or we won't be able to access them later
            var existingRecipe = _context.Recipes
                                    .Include(r => r.Ingredients)
                                    .Include(r => r.Instructions)
                                    .FirstOrDefault(r => r.Id == recipe.Id);

            if (existingRecipe == null)
            {
                throw new ArgumentException("Recipe not found");
            }

            // compares _context ingredients and input ingredients, if they are not the same eg. changes was made. we clear the _context and add the input to the context, ensuring all changes are reflected
            // next we do the same for instructions
            if(!Enumerable.SequenceEqual(existingRecipe.Ingredients, recipe.Ingredients))
            {
                existingRecipe.Ingredients.Clear();

                foreach (var ingredient in recipe.Ingredients)
                    existingRecipe.Ingredients.Add(ingredient);

            }

            if(!Enumerable.SequenceEqual(existingRecipe.Instructions, recipe.Instructions))
            {
                existingRecipe.Instructions.Clear();

                foreach(var instruction in recipe.Instructions)
                    existingRecipe.Instructions.Add(instruction);
            }


            _context.SaveChanges();
        }



        public void DeleteRecipe(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
            }
        }


    }
}

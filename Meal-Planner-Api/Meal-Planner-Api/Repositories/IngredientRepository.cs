using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly DataContext _context;

        public IngredientRepository(DataContext context)
        {
            _context = context;
        }
        public Ingredient GetIngredient(int id)
        {
            return _context.Ingredients.FirstOrDefault(x => x.Id == id);
        }

        public Ingredient GetIngredient(string name)
        {
            return _context.Ingredients.FirstOrDefault(x => x.Name == name);
        }

        public ICollection<Ingredient> GetIngredientsFromRecipe(int recipeId)
        {
            // find recipe
            // include ingredient

            var recipe = _context.Recipes.Include(ri => ri.RecipeIngredients)
                .ThenInclude(i => i.Ingredient)
                .FirstOrDefault(x => x.Id == recipeId);

            // get ingredient from recipe

            var ingredient = recipe.RecipeIngredients.Select(i => i.Ingredient).ToList();

            return ingredient;
        }

        public ICollection<Ingredient> GetIngredients()
        {
            return _context.Ingredients.OrderBy(x => x.Id).ToList();
        }

        public bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(x => x.Id == id);
        }
    }
}

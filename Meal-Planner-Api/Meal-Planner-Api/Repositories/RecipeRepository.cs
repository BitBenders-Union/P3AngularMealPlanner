using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private DataContext _context;


        public RecipeRepository(DataContext context)
        {
            _context = context;
        }
        public Recipe GetRecipe(int id)
        {

            return _context.Recipes.Where(x => x.Id == id).FirstOrDefault();
        }

        public ICollection<Recipe> GetRecipes()
        {
            return _context.Recipes.OrderBy(x => x.Id).ToList();
        }
    }
}

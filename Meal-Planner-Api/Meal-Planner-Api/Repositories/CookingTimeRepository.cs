using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class CookingTimeRepository : ICookingTimeRepository
    {
        private readonly DataContext _context;

        public CookingTimeRepository(DataContext context)
        {
            _context = context;
        }
        public bool CookingTimeExists(int id)
        {
            return _context.CookingTimes.Any(ct => ct.Id == id);
        }

        public CookingTime GetCookingTime(int id)
        {
            return _context.CookingTimes.FirstOrDefault(ct => ct.Id == id);
        }

        public CookingTime GetCookingTimeForRecipe(int recipeId)
        {

            // finds the correct recipe
            var recipe = _context.Recipes
                            .Include(rct => rct.RecipeCookingTime)
                            .ThenInclude(ct => ct.CookingTime)
                            .FirstOrDefault(r => r.Id == recipeId);

            // finds cookingtime from recipe
            var cookingTime = recipe.RecipeCookingTime
                            .Select(ct => ct.CookingTime)
                            .FirstOrDefault();

            // returns cookingtime
            return cookingTime;
        }

        public ICollection<CookingTime> GetCookingTimes()
        {
            return _context.CookingTimes.OrderBy(ct => ct.Id).ToList();
        }
    }
}

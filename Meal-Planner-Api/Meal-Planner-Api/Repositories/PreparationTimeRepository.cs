using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class PreparationTimeRepository : IPreparationTimeRepository
    {
        private readonly DataContext _context;

        public PreparationTimeRepository(DataContext context)
        {
            _context = context;
        }
        public PreparationTime GetPreparationTime(int id)
        {
            return _context.PreparationTimes.FirstOrDefault(x => x.Id == id);
        }

        public PreparationTime GetPreparationTimeFromRecipe(int recipeId)
        {

            var recipe = _context.Recipes.Include(x => x.RecipePreparationTime)
                .ThenInclude(x => x.PreparationTime)
                .FirstOrDefault(x => x.Id == recipeId);

            var prepTime = recipe.RecipePreparationTime.Select(x => x.PreparationTime)
                .Where(prep => prep != null)
                .FirstOrDefault();

            return prepTime;


        }

        public ICollection<PreparationTime> GetPreparationTimes()
        {
            return _context.PreparationTimes.OrderBy(x => x.Id).ToList();
        }

        public bool PreparationTimeExists(int id)
        {
            return _context.PreparationTimes.Any(x => x.Id == id);
        }
    }
}

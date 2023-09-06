using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class RecipeScheduleRepository : IRecipeScheduleRepository
    {
        private readonly DataContext _context;

        public RecipeScheduleRepository(DataContext context)
        {
            _context = context;
        }
        public RecipeSchedule GetRecipeScheduleForUser(int userId)
        {
            var recipeScheudle = _context.RecipeSchedules.Include(x => x.User)
                .FirstOrDefault(x => x.User.Id == userId);

            return recipeScheudle;
        }

        public ICollection<RecipeSchedule> GetRecipeSchedules()
        {
            return _context.RecipeSchedules.OrderBy(x => x.Id).ToList();
        }

        public RecipeSchedule GetRecipeSchedule(int recipeScheduleId)
        {
            return _context.RecipeSchedules.FirstOrDefault(x => x.Id == recipeScheduleId);
        }

        public bool RecipeScheduleExists(int userId)
        {
            // finds recipe schedule where user.id is == userId
            var recipeSchedule = _context.RecipeSchedules.Include(x => x.User)
                .Select(x => x.User.Id == userId);

            // returns true if any exists
            return recipeSchedule.Any();
        }
    }
}

using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
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

        public bool CookingTimeExists(CookingTimeDTO cookingTime)
        {
            return _context.CookingTimes.Any(ct => ct.Minutes == cookingTime.Minutes);

        }

        public bool CreateCookingTime(CookingTime cookingTime)
        {
            _context.Add(cookingTime);
            return Save();
        }

        public CookingTime GetCookingTime(int id)
        {
            return _context.CookingTimes.FirstOrDefault(ct => ct.Id == id);
        }

        public CookingTime GetCookingTimeForRecipe(int recipeId)
        {
            var cookingTime = _context.Recipes.Include(c => c.CookingTime).FirstOrDefault(x => x.Id == recipeId)?.CookingTime;

            if (cookingTime == null)
                return null;

            return cookingTime;
        }

        public CookingTime GetCookingTimeFromMinutes(int minutes)
        {
            return _context.CookingTimes.FirstOrDefault(x => x.Minutes == minutes);
        }

        public ICollection<CookingTime> GetCookingTimes()
        {
            return _context.CookingTimes.OrderBy(ct => ct.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

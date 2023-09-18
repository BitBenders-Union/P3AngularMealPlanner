using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
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

        public bool CreatePreparationTime(PreparationTime preparationTime)
        {
            _context.Add(preparationTime);
            return Save();
        }

        public PreparationTime GetPreparationTime(int id)
        {
            return _context.PreparationTimes.FirstOrDefault(x => x.Id == id);
        }

        public PreparationTime GetPreparationTimeFromMinutes(int minutes)
        {
            return _context.PreparationTimes.FirstOrDefault(x => x.Minutes == minutes);
        }

        public PreparationTime GetPreparationTimeFromRecipe(int recipeId)
        {

            var prepTime = _context.Recipes.Include(x => x.PreparationTime).FirstOrDefault(x => x.Id == recipeId)?.PreparationTime;

            if (prepTime == null)
                return null;

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
        
        public bool PreparationTimeExists(PreparationTimeDTO prepTime)
        {
            return _context.PreparationTimes.Any(x => x.Minutes == prepTime.Minutes);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }




    }
}

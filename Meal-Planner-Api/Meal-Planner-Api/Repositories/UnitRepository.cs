using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly DataContext _context;

        public UnitRepository(DataContext context)
        {
            _context = context;
        }
        public Unit GetUnitById(int id)
        {
            throw new NotImplementedException();
        }

        public Unit GetUnitByName(string name)
        {
            throw new NotImplementedException();
        }

        public Unit GetUnitForRecipe(int recipeId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Unit> GetUnits()
        {
            throw new NotImplementedException();
        }

        public bool UnitExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}

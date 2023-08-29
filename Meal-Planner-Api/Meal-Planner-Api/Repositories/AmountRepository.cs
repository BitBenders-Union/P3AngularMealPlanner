using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Repositories
{
    public class AmountRepository : IAmountRepository
    {
        private readonly DataContext _context;

        public AmountRepository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Amount> GetAllAmounts()
        {
            return _context.Amounts.ToList();
        }
    }
}

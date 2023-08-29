using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IAmountRepository
    {
        IEnumerable<Amount> GetAllAmounts();
    }
}

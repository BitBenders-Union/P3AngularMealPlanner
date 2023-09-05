using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IAmountRepository
    {
        ICollection<Amount> GetAmounts();
        Amount GetAmount(int id);
        bool AmountExists(int id);

    }
}

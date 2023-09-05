using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IPreparationTimeRepository
    {
        ICollection<PreparationTime> GetPreparationTimes();
        PreparationTime GetPreparationTime(int id);
        bool PreparationTimeExists(int id);
    }
}

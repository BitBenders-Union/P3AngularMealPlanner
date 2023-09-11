using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IPreparationTimeRepository
    {
        ICollection<PreparationTime> GetPreparationTimes();
        PreparationTime GetPreparationTime(int id);
        PreparationTime GetPreparationTimeFromRecipe(int recipeId);
        bool PreparationTimeExists(int id);

    }
}

using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface ICookingTimeRepository
    {
        ICollection<CookingTime> GetCookingTimes();
        CookingTime GetCookingTime(int id);
        CookingTime GetCookingTimeForRecipe(int recipeId);
        bool CookingTimeExists(int id);   
    }
}

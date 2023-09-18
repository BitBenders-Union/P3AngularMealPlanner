using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface ICookingTimeRepository
    {
        ICollection<CookingTime> GetCookingTimes();
        CookingTime GetCookingTime(int id);
        CookingTime GetCookingTimeForRecipe(int recipeId);
        CookingTime GetCookingTimeFromMinutes(int minutes);
        bool CookingTimeExists(int id);
        bool CookingTimeExists(CookingTimeDTO cookingTime);
        bool CreateCookingTime(CookingTime cookingTime);
        bool Save();
    }
}

using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IRecipeScheduleRepository
    {
        RecipeSchedule GetRecipeScheduleForUser(int userId);
        RecipeSchedule GetRecipeScheudle(int recipeScheduleId);
        ICollection<RecipeSchedule> GetRecipeSchedules();
        bool RecipeScheduleExists(int userId);
    }
}

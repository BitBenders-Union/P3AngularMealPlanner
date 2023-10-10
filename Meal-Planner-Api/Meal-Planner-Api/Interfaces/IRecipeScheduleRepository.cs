namespace Meal_Planner_Api.Interfaces
{
    public interface IRecipeScheduleRepository
    {
        RecipeSchedule GetRecipeScheduleForUser(int userId);
        RecipeSchedule GetRecipeSchedule(int recipeScheduleId);
        RecipeSchedule GetRecipeSchedule(int userId, int row, int col);
        ICollection<RecipeSchedule> GetRecipeSchedules();
        bool RecipeScheduleExists(int userId);
        bool CreateRecipeSchedule(RecipeSchedule recipeSchedule);
        bool UpdateRecipeInSchedule(RecipeSchedule recipeSchedule, int? recipeId);
        bool Save();
    }
}

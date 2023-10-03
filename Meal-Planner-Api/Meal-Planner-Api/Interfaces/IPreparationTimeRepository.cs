namespace Meal_Planner_Api.Interfaces
{
    public interface IPreparationTimeRepository
    {
        ICollection<PreparationTime> GetPreparationTimes();
        PreparationTime GetPreparationTime(int id);
        PreparationTime GetPreparationTimeFromRecipe(int recipeId);
        PreparationTime GetPreparationTimeFromMinutes(int minutes);
        bool PreparationTimeExists(PreparationTimeDTO prepTime);
        bool PreparationTimeExists(int id);
        bool CreatePreparationTime(PreparationTime preparationTime);
        bool Save();

    }
}

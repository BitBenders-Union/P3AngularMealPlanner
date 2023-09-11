using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IServingsRepository
    {
        Servings GetServing(int id);
        Servings GetServingForRecipe(int recipeId);
        ICollection<Servings> GetServings();
        bool servingExist(int id);
        bool CreateServing(Servings serving);
        bool Save();
    }
}

using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IRecipeRepository
    {
        ICollection<Recipe> GetRecipes();
        Recipe GetRecipe(int id);


    }
}

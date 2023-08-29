using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IRecipeRepository
    {
        Recipe GetRecipe(int id);
        IEnumerable<Recipe> GetAllRecipes();
        Recipe AddRecipe(Recipe recipe);
        void UpdateRecipe(Recipe recipe);
        void DeleteRecipe(int id);
    }
}

using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IRecipeRepository
    {
        ICollection<Recipe> GetRecipes();
        ICollection<Recipe> GetUserRecipes(int userId);
        Recipe GetRecipe(int id);
        Recipe GetRecipe(string name);
        float GetRecipeRating(int recipeId);
        bool RecipeExists(int recipeId);
        bool CreateRecipe(Recipe recipe, List<int> ratingIds, List<int> ingredientIds);
        bool Save();

    }
}

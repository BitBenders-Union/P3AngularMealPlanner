namespace Meal_Planner_Api.Interfaces
{
    public interface IRecipeRepository
    {
        ICollection<Recipe> GetRecipes();
        ICollection<Ingredient> GetIngredients(int recipeId);
        ICollection<Recipe> GetUserRecipes(int userId);
        Recipe GetRecipe(int id);
        Recipe GetRecipe(string name);
        int GetRecipeId(string name);
        float GetRecipeRating(int recipeId);
        bool RecipeExists(int recipeId);
        bool CreateRecipe(Recipe recipe, List<int> ratingIds, List<int> ingredientIds);
        bool CreateRecipeTest(Recipe recipe);
        bool UpdateRecipe(Recipe recipe);
        bool DeleteRecipe(Recipe recipe);
        bool Save();

    }
}

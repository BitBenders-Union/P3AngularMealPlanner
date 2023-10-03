namespace Meal_Planner_Api.Interfaces
{
    public interface IServingsRepository
    {
        Servings GetServing(int id);
        Servings GetServingForRecipe(int recipeId);
        Servings GetServingsFromQuantity(int quantity);
        ICollection<Servings> GetServings();
        bool servingExist(int id);
        bool servingExist(ServingsDTO servings);
        bool CreateServing(Servings serving);
        bool Save();
    }
}

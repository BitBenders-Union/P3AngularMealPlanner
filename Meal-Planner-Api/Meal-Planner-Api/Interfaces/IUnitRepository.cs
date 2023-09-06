using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IUnitRepository
    {
        ICollection<Unit> GetUnits();
        Unit GetUnitById(int id);
        Unit GetUnitByName(string name);
        Unit GetUnitForRecipe(int recipeId);
        Unit GetUnitFromIngredient(int ingredientId);
        bool UnitExists(int id);
    }
}

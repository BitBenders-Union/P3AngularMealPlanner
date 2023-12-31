﻿namespace Meal_Planner_Api.Interfaces
{
    public interface IUnitRepository
    {
        ICollection<Unit> GetUnits();
        Unit GetUnitById(int id);
        Unit GetUnitByName(string name);
        Unit GetUnitForRecipe(int recipeId);
        Unit GetUnitFromIngredient(int ingredientId);
        bool UnitExists(int id);
        bool UnitExists(string name);
        bool CreateUnit(Unit unit);
        bool Save();
    }
}

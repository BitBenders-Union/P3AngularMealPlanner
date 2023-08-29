using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IIngredientRepository
    {
        Ingredient CreateIngredient(Ingredient ingredient);
    }
}

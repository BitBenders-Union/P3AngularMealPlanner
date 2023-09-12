using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IAmountRepository
    {
        ICollection<Amount> GetAmounts();
        Amount GetAmount(int id);
        ICollection<Amount> GetAmountsFromRecipe(int recipeId);
        Amount GetAmountForIngredient(int ingredientId);
        bool AmountExists(int id);
        bool CreateAmount(Amount amount);
        bool Save();

    }
}

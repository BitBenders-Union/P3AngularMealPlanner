using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IAmountRepository
    {
        ICollection<Amount> GetAmounts();
        Amount GetAmount(int id);
        Amount GetAmountByQuantity(float quantity);
        ICollection<Amount> GetAmountsFromRecipe(int recipeId);
        Amount GetAmountForIngredient(int ingredientId);
        bool AmountExists(int id);
        bool AmountExistByQuantity(float quantity);
        bool CreateAmount(Amount amount);
        bool DeleteAmount(Amount amount);
        bool UpdateAmount(Amount amount);
        bool Save();

    }
}

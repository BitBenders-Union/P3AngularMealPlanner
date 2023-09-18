namespace Meal_Planner_Api.Models
{
    public class IngredientAmount
    {
        public int ingredientId { get; set; }
        public int amountId { get; set; }
        public Ingredient ingredient { get; set; }
        public Amount amount { get; set; }

    }
}

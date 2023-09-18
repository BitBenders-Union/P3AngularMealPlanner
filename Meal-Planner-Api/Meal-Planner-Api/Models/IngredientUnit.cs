namespace Meal_Planner_Api.Models
{
    public class IngredientUnit
    {
        public int ingredientId { get; set; }
        public int unitId { get; set; }
        public Ingredient ingredient { get; set; }
        public Unit unit { get; set; }

    }
}

namespace Meal_Planner_Api.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Amount Amount { get; set; }
        public Unit Unit { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

    }
}

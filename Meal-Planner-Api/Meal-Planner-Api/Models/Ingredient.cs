namespace Meal_Planner_Api.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Amount Amounts { get; set; }
        public int RecipeId { get; set; }
    }
}

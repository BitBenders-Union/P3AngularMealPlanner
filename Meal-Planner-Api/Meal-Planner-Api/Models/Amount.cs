namespace Meal_Planner_Api.Models
{
    public class Amount
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public int IngredientId { get; set; }
    }
}

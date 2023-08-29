namespace Meal_Planner_Api.Models
{
    public class Instruction
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int RecipeId { get; set; }
    }
}

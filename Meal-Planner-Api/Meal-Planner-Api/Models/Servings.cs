namespace Meal_Planner_Api.Models
{
    public class Servings
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ICollection<Recipe> Recipe { get; set; }


    }
}

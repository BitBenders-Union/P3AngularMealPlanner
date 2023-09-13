namespace Meal_Planner_Api.Models
{
    public class PreparationTime
    {
        public int Id { get; set; }
        public int Minutes { get; set; }
        public ICollection<Recipe> Recipe { get; set; }

    }
}

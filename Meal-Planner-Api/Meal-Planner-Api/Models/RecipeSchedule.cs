namespace Meal_Planner_Api.Models
{
    public class RecipeSchedule
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public User User { get; set; }
    }
}

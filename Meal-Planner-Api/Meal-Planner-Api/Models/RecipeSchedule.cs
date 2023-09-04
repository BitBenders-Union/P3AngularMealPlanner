namespace Meal_Planner_Api.Models
{
    public class RecipeSchedule
    {
        public int RecipeScheduleID { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public ICollection<Recipe> RecipeID { get; set; }
        public User User { get; set; }
    }
}

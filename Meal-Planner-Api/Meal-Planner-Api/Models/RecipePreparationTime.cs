namespace Meal_Planner_Api.Models
{
    public class RecipePreparationTime
    {
        public int RecipeID { get; set; }
        public int PreparationTimeID { get; set; }
        public Recipe Recipe { get; set; }
        public PreparationTime PreparationTime { get; set; }

    }
}

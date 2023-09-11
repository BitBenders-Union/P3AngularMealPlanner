namespace Meal_Planner_Api.Models
{
    public class RecipeServings
    {
        public int RecipeID { get; set; }
        public int ServingID { get; set; }
        public Recipe Recipe { get; set; }
        public Servings Servings { get; set; }

    }
}

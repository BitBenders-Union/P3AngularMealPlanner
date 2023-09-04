namespace Meal_Planner_Api.Models
{
    public class RecipeCookingTime
    {
        public int RecipeID { get; set; }
        public int CooktinTimeID { get; set; }
        public Recipe Recipe { get; set; }
        public CookingTime CookingTime { get; set; }

    }
}

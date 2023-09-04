namespace Meal_Planner_Api.Models
{
    public class UserRecipeRating
    {
        public int UserRecipeRatingID { get; set; }
        public int UserID { get; set; }
        public int RecipeID { get; set; }
        public int RatingID { get; set; }
    }
}

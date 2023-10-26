namespace Meal_Planner_Api.Models
{
    public class RecipeRating
    {
        public int RecipeRatingId { get; set; }
        public int RecipeID { get; set; }
        public int RatingID { get; set; }
        public int UserID { get; set; }
        public Recipe Recipe { get; set; }
        public Rating Rating { get; set; }
        public User User { get; set; }
    }
}

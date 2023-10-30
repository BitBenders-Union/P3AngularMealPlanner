namespace Meal_Planner_Api.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public float Score { get; set; }
        public ICollection<RecipeRating> RecipeRating { get; set; }

    }
}

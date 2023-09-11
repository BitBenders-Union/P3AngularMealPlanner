namespace Meal_Planner_Api.Models
{
    public class UserRating
    {
        public int UserId { get; set; }
        public int RatingId { get; set; }
        public User User { get; set; }
        public Rating Rating { get; set; }

    }
}

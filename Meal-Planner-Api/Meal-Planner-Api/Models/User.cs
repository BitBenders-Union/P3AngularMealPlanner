namespace Meal_Planner_Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

        //TODO: make binding to rating here and in rating make binding to user
    }
}

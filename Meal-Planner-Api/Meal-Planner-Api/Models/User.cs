namespace Meal_Planner_Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } // Renamed to match class name
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}

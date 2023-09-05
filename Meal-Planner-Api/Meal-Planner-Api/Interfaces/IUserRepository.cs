using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUser(string username);
        User ValidatePassword(byte[] hashedPassword);
        bool UserExists(int id);
    }
}

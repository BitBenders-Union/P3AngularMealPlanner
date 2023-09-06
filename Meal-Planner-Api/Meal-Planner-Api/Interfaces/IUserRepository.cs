using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUser(string username);
        bool ValidateUser(byte[] hashedPassword, string username);
        bool UserExists(int id);
        bool UserExists(string username);
    }
}

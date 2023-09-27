using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUser(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(x => x.Id).ToList();
        }

        public bool Save()
        {
            var saved =_context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(x => x.Id == id);
        }

        public bool UserExists(string username)
        {
            return _context.Users.Any(x => x.Username.Trim().ToLower() == username.Trim().ToLower());
        }

        public bool ValidateUser(byte[] hashedPassword, string username)
        {
            bool user = _context.Users.Any(x => x.Username == username);
            bool psw = _context.Users.Any(y => y.PasswordHash == hashedPassword);

            if(!user)
                return false;
            else if(!psw)
                return false;
            return true;

        }
    }
}

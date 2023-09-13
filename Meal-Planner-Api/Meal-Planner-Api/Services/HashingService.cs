using Konscious.Security.Cryptography;
using Meal_Planner_Api.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Meal_Planner_Api.Services
{
    public class HashingService : IHashingService
    {
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = new 
                RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public byte[] PasswordHashing(string password, byte[] salt)
        {
            Argon2id argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 4;
            argon2.Iterations = 2;
            argon2.MemorySize = 1024;
            argon2.DegreeOfParallelism = 2;

            byte[] hash = argon2.GetBytes(32);

            return hash;
        }
    }
}

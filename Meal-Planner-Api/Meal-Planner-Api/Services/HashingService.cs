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
            Argon2id argon2 = new(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 4,
                Iterations = 2,
                MemorySize = 1024
            };
            

            byte[] hash = argon2.GetBytes(32);

            return hash;
        }
    }
}

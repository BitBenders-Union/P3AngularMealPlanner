namespace Meal_Planner_Api.Interfaces
{
    public interface IHashingService
    {
        byte[] PasswordHashing(string password, byte[] salt);
        byte[] GenerateSalt();
    }
}
 
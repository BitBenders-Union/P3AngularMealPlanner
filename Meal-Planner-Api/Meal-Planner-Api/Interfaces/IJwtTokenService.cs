using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IJwtTokenService
    {
        string CreateJwtToken(User user);
    }
}

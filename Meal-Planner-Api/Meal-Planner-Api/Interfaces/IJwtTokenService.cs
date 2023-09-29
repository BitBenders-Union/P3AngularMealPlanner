using Meal_Planner_Api.Models;
using System.Security.Claims;

namespace Meal_Planner_Api.Interfaces
{
    public interface IJwtTokenService
    {
        string CreateJwtToken(User user);
        string CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

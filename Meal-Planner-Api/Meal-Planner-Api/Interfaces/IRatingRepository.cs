using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IRatingRepository
    {
        // get all ratings for a recipe
        ICollection<Rating> GetRatingsForRecipe(int recipeId);

        // Get all ratings for a user
        ICollection<Rating> GetRatingsForUser(int userId);

        //get a rating from a user for a certain recipe
        Rating GetRecipeUserRating(int userID, int recipeId);

        // get a rating by rating ID
        Rating GetRating(int id);

        // get all ratings
        ICollection<Rating> GetRatings();

        bool recipeRatingsExists(int recipeId);
        bool ratingExists(int id);
    }
}

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
        Rating GetRecipeRating(int recipeId);

        // get a rating by rating ID
        Rating GetRating(int id);
        Rating GetRatingFromScore(float score);
        int GetRatingId(float value);

        // get all ratings
        ICollection<Rating> GetRatings();

        bool recipeRatingsExists(int userId, int recipeId);
        bool ratingExists(float score);
        bool ratingExists(ICollection<RatingDTO> rating);
        RecipeRating GetRecipeRating(int userId, int recipeId);
        bool CreateRating(Rating rating);
        bool CreateRecipeRating(RecipeRating recipeRating);
        bool UpdateRecipeRating(RecipeRating recipeRating);
        bool DeleteRating(Rating rating);
        bool UpdateRating(Rating rating);
        bool Save();
    }
}

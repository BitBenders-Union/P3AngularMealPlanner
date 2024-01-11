using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly DataContext _context;

        public RatingRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateRating(Rating rating)
        {
            _context.Add(rating);
            return Save();
        }



        public bool DeleteRating(Rating rating)
        {
            _context.Remove(rating);
            return Save();
        }

        public Rating GetRating(int id)
        {
            return _context.Ratings.FirstOrDefault(x => x.Id == id);
        }

        public Rating GetRatingFromScore(float score)
        {
            return _context.Ratings.FirstOrDefault(x => x.Score == score);
        }

        public int GetRatingId(float value)
        {
            return _context.Ratings.FirstOrDefault(x => x.Score == value).Id;
        }

        public ICollection<Rating> GetRatings()
        {
            return _context.Ratings.OrderBy(x => x.Id).ToList();
        }

        public ICollection<Rating> GetRatingsForRecipe(int recipeId)
        {

            var rating = _context.Ratings.Include(x => x.RecipeRating)
                .Where(x => x.RecipeRating.Any(x => x.RecipeID == recipeId)).ToList();
                

            return rating;


        }

        public ICollection<Rating> GetRatingsForUser(int userId)
        {
            // returns all the ratings the user have given

            var user = _context.Users.Include(x => x.RecipeRatings)
                .ThenInclude(x => x.Rating)
                .FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return null;

            var rating = user.RecipeRatings.Select(x => x.Rating)
                .Where(x => x != null).ToList();

            return rating;
        }

        public Rating GetRecipeRating(int recipeId)
        {
            // find all ratings for the recipe
            var ratings = _context.Ratings.Include(x => x.RecipeRating)
                .Where(x => x.RecipeRating.Any(x => x.RecipeID == recipeId)).ToList();

            if (ratings.Count <= 0)
                return null;

            Rating rating = new()
            {
                Score = ratings.Average(x => x.Score)
            };

            return rating;
        }

        public Rating GetRecipeUserRating(int userID, int recipeId)
        {
            // get the rating a user made on a recipe

            //var userRatings = GetRatingsForUser(userID);

            // find the recipe inside the userRatings



            //TODO: After implementing user and rating binding, and the GetRatingsForUser method, complete this method.
            throw new NotImplementedException();

        }

        public bool ratingExists(float score)
        {
            return _context.Ratings.Any(x => x.Score == score);
        }

        public bool ratingExists(ICollection<RatingDTO> rating)
        {

            return _context.Ratings.Any(x => x.Score == rating.First().Score);
        }

        public bool recipeRatingsExists(int userId, int recipeId)
        {
            return _context.RecipeRatings.Any(x => x.RecipeID == recipeId && x.UserID == userId);
        }

        public bool CreateRecipeRating(RecipeRating recipeRating)
        {

            _context.Add(recipeRating);
            return Save();
        }

        public RecipeRating GetRecipeRating(int userId, int recipeId)
        {
            return _context.RecipeRatings.FirstOrDefault(x => x.RecipeID == recipeId && x.UserID == userId);
        }

        public bool UpdateRecipeRating(RecipeRating recipeRating)
        {
            var existingRecipeRating = _context.RecipeRatings.FirstOrDefault(x => x.RecipeID == recipeRating.RecipeID && x.UserID == recipeRating.UserID);
            existingRecipeRating.Rating = recipeRating.Rating;
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateRating(Rating rating)
        {
            throw new NotImplementedException();
        }
    }
}

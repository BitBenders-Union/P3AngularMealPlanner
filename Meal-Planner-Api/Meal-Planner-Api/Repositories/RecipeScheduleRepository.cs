namespace Meal_Planner_Api.Repositories
{
    public class RecipeScheduleRepository : IRecipeScheduleRepository
    {
        private readonly DataContext _context;

        public RecipeScheduleRepository(DataContext context)
        {
            _context = context;
        }
        public RecipeSchedule GetRecipeScheduleForUser(int userId)
        {
            // this can't work properly since a user has 21 entries in the recipe schedule table
            // I guess it just takes the first
            var recipeScheudle = _context.RecipeSchedules.Include(x => x.User)
                .FirstOrDefault(x => x.User.Id == userId);

            return recipeScheudle;
        }

        public ICollection<RecipeSchedule> GetRecipeSchedules()
        {
            return _context.RecipeSchedules.OrderBy(x => x.Id).Include(x => x.User).ToList();
        }

        public RecipeSchedule GetRecipeSchedule(int recipeScheduleId)
        {
            return _context.RecipeSchedules.Include(x => x.User).FirstOrDefault(x => x.Id == recipeScheduleId);
        }

        public bool RecipeScheduleExists(int userId)
        {
            // finds recipe schedule where user.id is == userId
            var recipeSchedule = _context.RecipeSchedules.Include(x => x.User)
                .Select(x => x.User.Id == userId);

            // returns true if any exists
            return recipeSchedule.Any();
        }

        public bool CreateRecipeSchedule(RecipeSchedule recipeSchedule)
        {
            _context.Add(recipeSchedule);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRecipeInSchedule(RecipeSchedule recipeSchedule, int? recipeId)
        {
            recipeSchedule.RecipeId = recipeId;
            _context.Update(recipeSchedule);
            return Save();
        }

        public RecipeSchedule GetRecipeSchedule(int userId, int row, int col)
        {
            return _context.RecipeSchedules.FirstOrDefault(x => x.User.Id == userId && x.Row == row && x.Column == col);
        }
    }
}

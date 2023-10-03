namespace Meal_Planner_Api.Repositories
{
    public class ServingsRepository : IServingsRepository
    {
        private readonly DataContext _context;

        public ServingsRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateServing(Servings serving)
        {
            _context.Add(serving);
            return Save();
        }

        public Servings GetServing(int id)
        {
            return _context.Servings.FirstOrDefault(x => x.Id == id);
        }

        public Servings GetServingForRecipe(int recipeId)
        {
            var serving = _context.Recipes.Include(x => x.Servings).FirstOrDefault(x => x.Id == recipeId)?.Servings;

            if (serving == null)
                return null;

            return serving;
        }

        public ICollection<Servings> GetServings()
        {
            return _context.Servings.OrderBy(x => x.Id).ToList();
        }

        public Servings GetServingsFromQuantity(int quantity)
        {
            return _context.Servings.FirstOrDefault(x => x.Quantity == quantity);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool servingExist(int id)
        {
            return _context.Servings.Any(x => x.Id == id);
        }

        public bool servingExist(ServingsDTO servings)
        {
            return _context.Servings.Any(x => x.Quantity == servings.Quantity);

        }
    }
}

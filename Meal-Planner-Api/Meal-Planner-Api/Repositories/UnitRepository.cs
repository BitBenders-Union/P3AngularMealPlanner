namespace Meal_Planner_Api.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly DataContext _context;

        public UnitRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUnit(Unit unit)
        {
            _context.Add(unit);
            return Save();
        }

        public Unit GetUnitById(int id)
        {
            return _context.Units.FirstOrDefault(u => u.Id == id);
        }

        public Unit GetUnitByName(string name)
        {
            return _context.Units.FirstOrDefault(u => u.Measurement == name);

        }

        public Unit GetUnitForRecipe(int recipeId)
        {
            //var recipe = _context.Recipes
            //                        .Include(ri => ri.RecipeIngredients)
            //                        .ThenInclude(i => i.Ingredient)
            //                        .FirstOrDefault(r => r.Id == recipeId);

            //var unit = recipe.RecipeIngredients.Select(iu => iu.Ingredient.IngredientUnit.FirstOrDefault(x => x.unit))
            //    .Where(unit => unit != null)
            //    .FirstOrDefault();

            //return unit;

            throw new NotImplementedException();

        }

        public Unit GetUnitFromIngredient(int ingredientId)
        {
            //return _context.Ingredients.Include(u => u.Unit)
            //        .FirstOrDefault(x => x.Id == ingredientId)
            //        .Unit;

            throw new NotImplementedException();
        }

        public ICollection<Unit> GetUnits()
        {
            return _context.Units.OrderBy(u => u.Measurement).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UnitExists(int id)
        {
            return _context.Units.Any(u => u.Id == id);
        }

        public bool UnitExists(string name)
        {
            return _context.Units.Any(u => u.Measurement == name);
        }
    }
}

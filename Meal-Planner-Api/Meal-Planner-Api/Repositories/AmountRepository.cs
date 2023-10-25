namespace Meal_Planner_Api.Repositories
{
    public class AmountRepository : IAmountRepository
    {
        private readonly DataContext _context;

        public AmountRepository(DataContext context)
        {
            _context = context;
        }
        public bool AmountExists(int id)
        {
            return _context.Amounts.Any(a => a.Id == id);
        }

        public Amount GetAmount(int id)
        {
            return _context.Amounts.FirstOrDefault(a => a.Id == id);
        }

        public ICollection<Amount> GetAmountsFromRecipe(int recipeId)
        {
            
            // not implemented throw error
            throw new NotImplementedException();

        }

        public Amount GetAmountForIngredient(int ingredientId)
        {
            // not implemented throw error
            throw new NotImplementedException();

        }


        public ICollection<Amount> GetAmounts()
        {
            return _context.Amounts.OrderBy(a => a.Id).ToList();
        }

        public bool CreateAmount(Amount amount)
        {
            _context.Add(amount);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public Amount GetAmountByQuantity(float quantity)
        {
            var amount = _context.Amounts.FirstOrDefault(a => a.Quantity == quantity);
            return amount;
        }

        public bool AmountExistByQuantity(float quantity)
        {
            return _context.Amounts.Any(a => a.Quantity == quantity);
        }

        public bool DeleteAmount(Amount amount)
        {
            _context.Remove(amount);
            return Save();
        }

        public bool UpdateAmount(Amount amount)
        {
            _context.Update(amount);
            return Save();
        }
    }
}

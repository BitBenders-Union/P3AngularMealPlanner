using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

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

            return _context.Recipes.Where(r => r.Id == recipeId)
                .SelectMany(r => r.RecipeIngredients)
                .Select(i => i.Ingredient)
                .SelectMany(i => i.IngredientAmount)
                .Select(ia => ia.amount)
                .ToList();
            
        }

        public Amount GetAmountForIngredient(int ingredientId)
        {
            return _context.Ingredients
                .Where(i => i.Id == ingredientId)
                .SelectMany(i => i.IngredientAmount)
                .Select(ia => ia.amount)
                .FirstOrDefault();
                           
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
            return saved > 0 ? true : false;
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

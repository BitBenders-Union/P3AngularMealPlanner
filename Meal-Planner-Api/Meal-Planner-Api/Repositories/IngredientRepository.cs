using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly DataContext _context;
        private IAmountRepository _amountRepository;
        private IUnitRepository _unitRepository;

        public IngredientRepository(DataContext context, IAmountRepository amountRepository, IUnitRepository unitRepository)
        {
            _context = context;
            _amountRepository = amountRepository;
            _unitRepository = unitRepository;
        }
        public Ingredient GetIngredient(int id)
        {
            return _context.Ingredients.FirstOrDefault(x => x.Id == id);
        }

        public Ingredient GetIngredient(string name)
        {
            return _context.Ingredients.FirstOrDefault(x => x.Name == name);
        }

        public ICollection<Ingredient> GetIngredientsFromRecipe(int recipeId)
        {
            // find recipe
            // include ingredient

            var recipe = _context.Recipes.Include(ri => ri.RecipeIngredients)
                .ThenInclude(i => i.Ingredient)
                .FirstOrDefault(x => x.Id == recipeId);

            // get ingredient from recipe

            var ingredient = recipe.RecipeIngredients.Select(i => i.Ingredient).ToList();

            return ingredient;
        }

        public ICollection<Ingredient> GetIngredients()
        {
            return _context.Ingredients.Include(x => x.Amount)
                .Include(x=> x.Unit)
                .OrderBy(x => x.Id)
                .ToList();
        }

        public bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(x => x.Id == id);
        }
        public bool IngredientExists(string name)
        {
            return _context.Ingredients.Any(x => x.Name == name);
        }

        public bool CreateIngredient(Ingredient ingredient)
        {
            var existingIngredient = GetIngredient(ingredient.Name);
            if (existingIngredient != null)
            {
                return false;
            }

            // Check if an amount with the same quantity exists
            var existingAmount = _amountRepository.GetAmountByQuantity(ingredient.Amount.Quantity);

            // If there's an existing amount, use it; otherwise, create a new one
            if (existingAmount == null)
            {
                existingAmount = new Amount { Quantity = ingredient.Amount.Quantity };
                _context.Amounts.Add(existingAmount);
            }

            // Check if a unit with the same measurement exists
            var existingUnit = _unitRepository.GetUnitByName(ingredient.Unit.Measurement);

            // Create a new unit if it doesn't exist
            if (existingUnit == null)
            {
                existingUnit = new Unit { Measurement = ingredient.Unit.Measurement };
                _context.Units.Add(existingUnit);
            }

            // Set the existing amount and unit for the ingredient
            ingredient.Amount = existingAmount;
            ingredient.Unit = existingUnit;

            _context.Ingredients.Add(ingredient);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}

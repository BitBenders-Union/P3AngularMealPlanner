using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly DataContext _context;

        public IngredientRepository(DataContext context)
        {
            _context = context;
        }

        public Ingredient CreateIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient); // Add the ingredient to the DbSet
            _context.SaveChanges(); // Save changes to the database
            return ingredient; // Return the newly created ingredient
        }
    }
}

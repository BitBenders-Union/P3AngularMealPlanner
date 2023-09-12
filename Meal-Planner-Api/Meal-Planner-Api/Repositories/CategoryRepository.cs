using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoriesExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);

            return Save();

        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public Category GetCategoryByRecipeId(int recipeId)
        {
            // get all recipes
            // include the category
            // find the recipe from the id
            // return the recipe's category.
            var recipe = _context.Recipes.Include(c => c.category).FirstOrDefault(r => r.Id == recipeId);

            return recipe.category;
        }

        public bool Save()
        {
            // saves the changes and updates the database
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}

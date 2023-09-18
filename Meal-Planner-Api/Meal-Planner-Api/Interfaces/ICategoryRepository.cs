using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        Category GetCategoryFromName(string name);
        Category GetCategoryByRecipeId(int recipeId);
        bool CategoriesExists(int id);
        bool CategoriesExists(string name);

        bool CreateCategory(Category category);

        bool Save();

    }
}

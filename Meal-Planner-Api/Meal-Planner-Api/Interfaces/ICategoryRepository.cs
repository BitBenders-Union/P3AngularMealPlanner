using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        bool CategoriesExists(int id);

    }
}

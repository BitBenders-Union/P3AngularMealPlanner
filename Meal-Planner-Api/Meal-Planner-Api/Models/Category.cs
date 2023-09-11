namespace Meal_Planner_Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}

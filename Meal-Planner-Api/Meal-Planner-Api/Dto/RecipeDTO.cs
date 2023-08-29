using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Dto
{
    public class RecipeDTO
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public int Servings { get; set; }
        public double Rating { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Instruction> Instructions { get; set; }
        public bool Deleted { get; set; }
    }
}

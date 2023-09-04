namespace Meal_Planner_Api.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public PreparationTime PreparationTime { get; set; }
        public CookingTime CookingTime { get; set; }
        public Servings Servings { get; set; }
        public Rating Rating { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Instruction> Instructions { get; set; }
    }
}

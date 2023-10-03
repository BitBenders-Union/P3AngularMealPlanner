namespace Meal_Planner_Api.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Category category { get; set; }
        public PreparationTime PreparationTime { get; set; }
        public CookingTime CookingTime { get; set; }
        public Servings Servings { get; set; }
        public ICollection<RecipeRating> RecipeRating { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public ICollection<Instruction> Instructions { get; set; }
        public User User { get; set; }


    }
}

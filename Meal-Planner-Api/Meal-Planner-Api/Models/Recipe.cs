using Meal_Planner_Api.Dto;

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

        public int UserId { get; set; }
        //public User User { get; set; } //TODO: figure out how to only use username here, we don't want to insert hash and salt when creating a recipe, idea: create a new dto only containing userid and username
    }
}

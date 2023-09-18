namespace Meal_Planner_Api.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<IngredientAmount> IngredientAmount { get; set; }
        public ICollection<IngredientUnit> IngredientUnit { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

    }
}

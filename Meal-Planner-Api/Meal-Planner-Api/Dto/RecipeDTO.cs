using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Dto
{
    // limit data that users can get and send to and from database
    public class RecipeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CategoryDTO Category { get; set; }
        public PreparationTimeDTO PreparationTimes { get; set; }
        public CookingTimeDTO CookingTimes { get; set; }
        public ServingsDTO Servings { get; set; }
        public ICollection<RatingDTO> Ratings { get; set; }
        public ICollection<IngredientDTO> Ingredients { get; set; }
        public ICollection<InstructionDTO> Instructions { get; set; }
        public int UserId { get; set; }
    }
}

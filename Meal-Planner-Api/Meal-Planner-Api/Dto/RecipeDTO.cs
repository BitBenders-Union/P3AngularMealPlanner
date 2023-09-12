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
        public ICollection<PreparationTimeDTO> PreparationTimes { get; set; }
        public ICollection<CookingTimeDTO> CookingTimes { get; set; }
        public ICollection<ServingsDTO> Servings { get; set; }
        public ICollection<RatingDTO> Ratings { get; set; }
        public ICollection<IngredientDTO> Ingredients { get; set; }
        public ICollection<InstructionDTO> Instructions { get; set; }
        public UserOnlyNameDTO User { get; set; }
    }
}

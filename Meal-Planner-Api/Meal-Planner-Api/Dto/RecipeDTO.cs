﻿namespace Meal_Planner_Api.Dto
{
    // limit data that users can get and send to and from database
    public class RecipeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CategoryDTO Category { get; set; }
        public PreparationTimeDTO PreparationTime { get; set; }
        public CookingTimeDTO CookingTime { get; set; }
        public ServingsDTO Servings { get; set; }
        public ICollection<IngredientDTO> Ingredients { get; set; }
        public ICollection<InstructionDTO> Instructions { get; set; }
        public UserOnlyNameDTO User { get; set; }
    }
}

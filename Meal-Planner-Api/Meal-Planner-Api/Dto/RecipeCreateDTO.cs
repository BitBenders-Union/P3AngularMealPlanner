namespace Meal_Planner_Api.Dto
{
    public class RecipeCreateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int categoryId { get; set; }
        public int preparationTimeID { get; set; }
        public int cookingTimeID { get; set; }
        public int servingsID { get; set; }
        public ICollection<int> ratingsID { get; set; }
        public ICollection<int> ingredientsID { get; set; }
        public ICollection<InstructionDTO> instructions { get; set; }
        public int userID { get; set; }
    }
}

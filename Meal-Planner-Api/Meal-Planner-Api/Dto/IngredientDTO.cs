namespace Meal_Planner_Api.Dto
{
    public class IngredientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AmountDTO Amount { get; set; }
        public UnitDTO Unit { get; set; }

    }
}

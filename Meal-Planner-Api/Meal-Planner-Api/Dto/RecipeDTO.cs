using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Dto
{
    // limit data that users can get and send to and from database
    public class RecipeDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

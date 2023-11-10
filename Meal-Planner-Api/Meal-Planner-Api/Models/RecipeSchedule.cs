using System.ComponentModel.DataAnnotations.Schema;

namespace Meal_Planner_Api.Models
{
    public class RecipeSchedule
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int? RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }
        public User User { get; set; }
    }
}

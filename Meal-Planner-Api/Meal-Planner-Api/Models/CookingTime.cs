﻿namespace Meal_Planner_Api.Models
{
    public class CookingTime
    {
        public int Id { get; set; }
        public int Minutes { get; set; }
        public ICollection<Recipe> Recipe { get; set; }

    }
}

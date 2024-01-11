﻿namespace Meal_Planner_Api.Models
{
    public class Amount
    {
        public int Id { get; set; }
        public float Quantity { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}

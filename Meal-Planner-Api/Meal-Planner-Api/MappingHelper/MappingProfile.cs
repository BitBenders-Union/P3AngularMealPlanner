using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.MappingHelper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<Recipe, RecipeDTO>();
            CreateMap<RecipeDTO, Recipe>();

            CreateMap<Ingredient, IngredientDTO>();
            CreateMap<IngredientDTO, Ingredient>();

            CreateMap<Amount, AmountDTO>();
            CreateMap<AmountDTO, Amount>();
        }
    }
}

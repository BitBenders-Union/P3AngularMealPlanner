using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.MappingHelper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<Amount, AmountDTO>();
            CreateMap<AmountDTO, Amount>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Ingredient, IngredientDTO>();
            CreateMap<IngredientDTO, Ingredient>();

            CreateMap<Instruction, InstructionDTO>();
            CreateMap<InstructionDTO, Instruction>();

            CreateMap<PreparationTime, PreparationTimeDTO>();
            CreateMap<PreparationTimeDTO, PreparationTime>();

            CreateMap<Rating, RatingDTO>();
            CreateMap<RatingDTO, Rating>();


            CreateMap<Recipe, RecipeDTO>();
            CreateMap<RecipeDTO, Recipe>();

            CreateMap<RecipeSchedule, RecipeScheduleDTO>();
            CreateMap<RecipeScheduleDTO, RecipeScheduleDTO>();

            CreateMap<Servings, ServingsDTO>();
            CreateMap<ServingsDTO, ServingsDTO>();

            CreateMap<Unit, UnitDTO>();
            CreateMap<UnitDTO, Unit>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();


        }
    }
}

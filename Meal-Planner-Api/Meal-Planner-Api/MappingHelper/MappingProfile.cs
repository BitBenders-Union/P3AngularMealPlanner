namespace Meal_Planner_Api.MappingHelper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Amount, AmountDTO>();
            CreateMap<AmountDTO, Amount>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<IngredientDTO, Ingredient>();

            CreateMap<Instruction, InstructionDTO>();
            CreateMap<InstructionDTO, Instruction>();

            CreateMap<PreparationTime, PreparationTimeDTO>();
            CreateMap<PreparationTimeDTO, PreparationTime>();

            CreateMap<CookingTime, CookingTimeDTO>();
            CreateMap<CookingTimeDTO, CookingTime>();

            CreateMap<Rating, RatingDTO>();
            CreateMap<RatingDTO, Rating>();

            CreateMap<Recipe, RecipeDTO>();
            CreateMap<RecipeDTO, Recipe>();

            CreateMap<RecipeSchedule, RecipeScheduleDTO>();

            CreateMap<RecipeScheduleDTO, RecipeSchedule>();

            CreateMap<Servings, ServingsDTO>();
            CreateMap<ServingsDTO, Servings>();

            CreateMap<Unit, UnitDTO>();
            CreateMap<UnitDTO, Unit>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<UserOnlyNameDTO, UserDTO>();
            CreateMap<UserDTO, UserOnlyNameDTO>();

            CreateMap<User, UserOnlyNameDTO>();
            CreateMap<UserOnlyNameDTO, User>();



        }
    }
}

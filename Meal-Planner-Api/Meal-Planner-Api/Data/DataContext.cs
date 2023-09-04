using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Amount> Amount { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CookingTime> CookingTime { get; set; }
        public DbSet<Instruction> Instruction { get; set; }
        public DbSet<PreparationTime> PreparationTime { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<RecipeSchedule> RecipeSchedule { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Amount> Amounts { get; set; }
        public DbSet<UserRecipeRating> UserRecipeRating { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }


    }
}

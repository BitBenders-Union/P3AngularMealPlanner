using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Amount> Amounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CookingTime> CookingTimes { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<PreparationTime> PreparationTimes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RecipeSchedule> RecipeSchedules { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Servings> Servings { get; set; }

        public DbSet<RecipeCookingTime> RecipeCookingTimes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipePreparationTime> RecipePreparationTimes { get; set; }
        public DbSet<RecipeRating> RecipeRatings { get; set; }
        public DbSet<RecipeServings> RecipeServings { get; set; }
 

        // Customize the tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // tell what keys to bind on
            // then tell that first key is many to many & what fk they have
            // the tell second key is many to many & what fk they have

            modelBuilder.Entity<RecipeCookingTime>()
                .HasKey(rc => new { rc.RecipeID, rc.CookingTimeID });
            modelBuilder.Entity<RecipeCookingTime>()
                .HasOne(r => r.Recipe)
                .WithMany(rc => rc.RecipeCookingTime)
                .HasForeignKey(r => r.RecipeID);
            modelBuilder.Entity<RecipeCookingTime>()
                .HasOne(c => c.CookingTime)
                .WithMany(rc => rc.RecipeCookingTime)
                .HasForeignKey(c => c.CookingTimeID);


            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeID, ri.IngredientID });
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(r => r.Recipe)
                .WithMany(ri => ri.RecipeIngredients)
                .HasForeignKey(r => r.RecipeID);
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(i => i.Ingredient)
                .WithMany(ri => ri.RecipeIngredients)
                .HasForeignKey(i => i.IngredientID);


            modelBuilder.Entity<RecipePreparationTime>()
                .HasKey(rp => new {rp.RecipeID, rp.PreparationTimeID});
            modelBuilder.Entity<RecipePreparationTime>()
                .HasOne(r => r.Recipe)
                .WithMany(rp => rp.RecipePreparationTime)
                .HasForeignKey(r => r.RecipeID);
            modelBuilder.Entity<RecipePreparationTime>()
                .HasOne(p => p.PreparationTime)
                .WithMany(rp => rp.RecipePreparationTime)
                .HasForeignKey(p => p.PreparationTimeID);



            modelBuilder.Entity<RecipeRating>()
                .HasKey(rr => new {rr.RecipeID, rr.RatingID});
            modelBuilder.Entity<RecipeRating>()
                .HasOne(r => r.Recipe)
                .WithMany(rr => rr.RecipeRating)
                .HasForeignKey(r => r.RecipeID);
            modelBuilder.Entity<RecipeRating>()
                .HasOne(r => r.Rating)
                .WithMany(rr => rr.RecipeRating)
                .HasForeignKey(r => r.RatingID);



            modelBuilder.Entity<RecipeServings>()
                .HasKey(rs => new {rs.RecipeID, rs.ServingID});
            modelBuilder.Entity<RecipeServings>()
                .HasOne(r => r.Recipe)
                .WithMany(rs => rs.RecipeServings)
                .HasForeignKey(r => r.RecipeID);
            modelBuilder.Entity<RecipeServings>()
                .HasOne(s => s.Servings)
                .WithMany(rs => rs.RecipeServings)
                .HasForeignKey(s => s.ServingID);


            modelBuilder.Entity<UserRating>()
                .HasKey(ur => new {ur.UserId, ur.RatingId});
            modelBuilder.Entity<UserRating>()
                .HasOne(u => u.User)
                .WithMany(ur => ur.UserRating)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UserRating>()
                .HasOne(r => r.Rating)
                .WithMany(ur => ur.UserRating)
                .HasForeignKey(r => r.RatingId);



        }


    }
}

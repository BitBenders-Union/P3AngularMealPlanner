
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

        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeRating> RecipeRatings { get; set; }


        // Customize the tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // tell what keys to bind on
            // then tell that first key is many to many & what fk they have
            // the tell second key is many to many & what fk they have



            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Amount)
                .WithMany(a => a.RecipeIngredients)
                .HasForeignKey(ri => ri.AmountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Unit)
                .WithMany(u => u.RecipeIngredients)
                .HasForeignKey(ri => ri.UnitId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<RecipeRating>()
                .HasOne(rr => rr.Recipe)
                .WithMany()
                .HasForeignKey(rr => rr.RecipeID);

            modelBuilder.Entity<RecipeRating>()
                .HasOne(rr => rr.Rating)
                .WithMany(r => r.RecipeRating)
                .HasForeignKey(rr => rr.RatingID);

            modelBuilder.Entity<RecipeRating>()
                .HasOne(rr => rr.User)
                .WithMany(u => u.RecipeRatings)
                .HasForeignKey(rr => rr.UserID);


            base.OnModelCreating(modelBuilder);

        }


    }
}

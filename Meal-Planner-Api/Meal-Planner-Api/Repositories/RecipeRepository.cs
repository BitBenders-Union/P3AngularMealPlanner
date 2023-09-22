using AutoMapper;
using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class RecipeRepository: IRecipeRepository
    {
        private DataContext _context;
        private IMapper _mapper;

        public RecipeRepository(DataContext context, IMapper map)
        {
            _context = context;
            _mapper = map;
        }


        public Recipe GetRecipe(int id)
        {

            return _context.Recipes
                .Include(x => x.category)
                .Include(x => x.PreparationTime)
                .Include(x => x.CookingTime)
                .Include(x => x.Servings)
                .Include(x => x.RecipeRating)
                    .ThenInclude(z => z.Rating)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Ingredient)
                        .ThenInclude(y => y.IngredientUnit)
                            .ThenInclude(w => w.unit)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Ingredient)
                        .ThenInclude(y => y.IngredientAmount)
                            .ThenInclude(w => w.amount)
                .Include(x => x.Instructions)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);

        }
         
        public Recipe GetRecipe(string name)
        {
            return _context.Recipes
                .Include(x => x.category)
                .Include(x => x.PreparationTime)
                .Include(x => x.CookingTime)
                .Include(x => x.Servings)
                .Include(x => x.RecipeRating)
                    .ThenInclude(z => z.Rating)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Ingredient)
                        .ThenInclude(y => y.IngredientUnit)
                            .ThenInclude(w => w.unit)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Ingredient)
                        .ThenInclude(y => y.IngredientAmount)
                            .ThenInclude(w => w.amount)
                .Include(x => x.Instructions)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Title == name);

        }

        public float GetRecipeRating(int recipeId)
        {
            var recipe = _context.Recipes.Include(rr => rr.RecipeRating).FirstOrDefault(r => r.Id == recipeId); // finds the recipe from id

            if (recipe != null)
            {
                // get average rating
                float avgRating = recipe.RecipeRating.Average(rr => rr.Rating.Score);
                return avgRating;
            }
            else return 0.0f; // if recipe doesn't exist return 0.0f as rating
        }

        public ICollection<Recipe> GetRecipes()
        {
            return _context.Recipes.OrderBy(x => x.Id)
                .Include(x => x.category)
                .Include(x => x.PreparationTime)
                .Include(x => x.CookingTime)
                .Include(x => x.Servings)
                .Include(x => x.RecipeRating)
                    .ThenInclude(z => z.Rating)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Ingredient)
                        .ThenInclude(y => y.IngredientUnit)
                            .ThenInclude(w => w.unit)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Ingredient)
                        .ThenInclude(y => y.IngredientAmount)
                            .ThenInclude(w => w.amount)
                .Include(x => x.Instructions)
                .Include(x => x.User)
                .ToList();
        }

        public ICollection<Recipe> GetUserRecipes(int userId)
        {
            return _context.Recipes.Where(r => r.User.Id == userId).ToList();
        }

        public bool RecipeExists(int recipeId)
        {
            return _context.Recipes.Any(r => r.Id == recipeId);
        }
        public bool CreateRecipe(Recipe recipe, List<int> ratingIds, List<int> ingredientIds )
        {

            // foreach id in rating and ingredient
            // create a RecipeRating and RecipeIngredient
            // add them to context

            foreach(var id in ratingIds)
            {
                var rating = _context.Ratings.FirstOrDefault(r => r.Id == id);

                var recipeRating = new RecipeRating
                {
                    Rating = rating,
                    Recipe = recipe
                };
                _context.Add(recipeRating);

            }

            foreach (var id in ingredientIds)
            {
                var ingredient = _context.Ingredients.FirstOrDefault(i => i.Id == id);
                var recipeIngredient = new RecipeIngredient
                {
                    Recipe = recipe,
                    Ingredient = ingredient
                };
                _context.Add(recipeIngredient);
            }

            // add recipe to context


            _context.Add(recipe);
            
            // save


            return Save();
        }

        public bool Save()
        {
            // save changes updates the database with the current context.
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public ICollection<Ingredient> GetIngredients(int recipeId)
        {
            return _context.Recipes
                .Where(r => r.Id == recipeId)
                .SelectMany(r => r.RecipeIngredients.Select(ri => ri.Ingredient))
                .Include(i => i.IngredientAmount)
                    .ThenInclude(ia => ia.amount)
                .Include(i => i.IngredientUnit)
                    .ThenInclude(iu => iu.unit)
                .ToList();
        }

        public bool UpdateRecipe(Recipe recipe)
        {
            _context.Update(recipe);
            return Save();
        }

        public bool DeleteRecipe(Recipe recipe)
        {
            _context.Remove(recipe);
            return Save();
        }
    }
}

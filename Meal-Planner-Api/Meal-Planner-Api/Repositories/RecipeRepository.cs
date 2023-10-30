using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DataContext _context;

        public RecipeRepository(DataContext context)
        {
            _context = context;
        }


        public Recipe GetRecipe(int id)
        {
            var recipe = _context.Recipes
                    .Include(x => x.Category)
                    .Include(x => x.PreparationTime)
                    .Include(x => x.CookingTime)
                    .Include(x => x.Servings)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Ingredient)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Amount)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Unit)
                    .Include(x => x.Instructions)
                    .Include(x => x.User)
                    .Where(x => x.Id == id)
                    .OrderBy(r => r.RecipeIngredients.Select(ri => ri.Ingredient.Order).Min())
                    .FirstOrDefault();

            if (recipe != null)
            {
                recipe.RecipeIngredients = recipe.RecipeIngredients.OrderBy(ri => ri.Ingredient.Order).ToList();
            }

            return recipe;

        }

        public Recipe GetRecipe(string name)
        {
            var recipe = _context.Recipes
                    .Include(x => x.Category)
                    .Include(x => x.PreparationTime)
                    .Include(x => x.CookingTime)
                    .Include(x => x.Servings)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Ingredient)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Amount)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Unit)
                    .Include(x => x.Instructions)
                    .Include(x => x.User)
                    .Where(x => x.Title == name)
                    .OrderBy(r => r.RecipeIngredients.Select(ri => ri.Ingredient.Order).Min())
                    .FirstOrDefault();

            if (recipe != null)
            {
                recipe.RecipeIngredients = recipe.RecipeIngredients.OrderBy(ri => ri.Ingredient.Order).ToList();
            }

            return recipe;

        }

        public float GetRecipeRating(int recipeId)
        {

            var rating = _context.Ratings.Where(r => r.RecipeRating.Any(rr => rr.RecipeID == recipeId)).ToList();

            if (rating == null)
                return 0.0f;

            return rating.Average(x => x.Score);

        }

        public ICollection<Recipe> GetRecipes()
        {
            var recipes = _context.Recipes
                    .Include(x => x.Category)
                    .Include(x => x.PreparationTime)
                    .Include(x => x.CookingTime)
                    .Include(x => x.Servings)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Ingredient)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Unit)
                    .Include(x => x.RecipeIngredients)
                        .ThenInclude(z => z.Amount)
                    .Include(x => x.Instructions)
                    .Include(x => x.User)
                    .ToList();
            recipes.ForEach(r => r.RecipeIngredients = r.RecipeIngredients.OrderBy(ri => ri.Ingredient.Order).ToList());

            return recipes;
        }

        public ICollection<Recipe> GetUserRecipes(int userId)
        {
            return _context.Recipes.Where(r => r.User.Id == userId)
                .Include(x => x.Category)
                .Include(x => x.PreparationTime)
                .Include(x => x.CookingTime)
                .Include(x => x.Servings)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Ingredient)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Unit)
                .Include(x => x.RecipeIngredients)
                    .ThenInclude(z => z.Amount)
                .Include(x => x.Instructions)
                .Include(x => x.User).ToList();
        }

        public bool RecipeExists(int recipeId)
        {
            return _context.Recipes.Any(r => r.Id == recipeId);
        }

        public bool CreateRecipe(Recipe recipe, List<int> ratingIds, List<int> ingredientIds)
        {

            // forEach id in rating and ingredient
            // create a RecipeRating and RecipeIngredient
            // add them to context

            foreach (var id in ratingIds)
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
            return saved > 0;
        }

        public ICollection<Ingredient> GetIngredients(int recipeId)
        {
            return _context.Recipes
                .Where(r => r.Id == recipeId)
                .SelectMany(r => r.RecipeIngredients.Select(ri => ri.Ingredient))
                .Include(i => i.RecipeIngredients)
                    .ThenInclude(ia => ia.Amount)
                .Include(i => i.RecipeIngredients)
                    .ThenInclude(iu => iu.Unit)
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

        public int GetRecipeId(string name)
        {
            return _context.Recipes.FirstOrDefault(r => r.Title == name).Id;
        }

        public bool CreateRecipeTest(Recipe recipe)
        {
            _context.Add(recipe);
            return Save();
        }
    }
}

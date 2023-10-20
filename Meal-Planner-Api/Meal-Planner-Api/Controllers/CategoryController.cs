
namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // get all categories
        [HttpGet]
        public IActionResult Get()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());

            if (categories == null || categories.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(categories);
        }

        // get category by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_categoryRepository.CategoriesExists(id))
                return NotFound();

            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(category);
        }

        // get category from recipeId
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetCategoryByRecipeId(int recipeId)
        {
            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategoryByRecipeId(recipeId));

            if (!ModelState.IsValid)
                return BadRequest();

            if (category == null)
                return NotFound();

            return Ok(category);

        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest();


            // looks for other categories with the same name
            var category = _categoryRepository.GetCategories()
                .FirstOrDefault(c => c.CategoryName.Trim().ToUpper() == categoryCreate.CategoryName.Trim().ToUpper());

            // if a category with the same name as the input exists, return 422 status code
            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            // create the category if it returns false it didn't save properly, something went wrong.
            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            // after all checks passed return Ok
            return Ok();

        }


    }
}

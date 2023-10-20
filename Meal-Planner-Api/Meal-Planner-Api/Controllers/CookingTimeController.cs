
namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookingTimeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICookingTimeRepository _cookingTimeRepository;

        public CookingTimeController(IMapper mapper, ICookingTimeRepository cookingTimeRepository)
        {
            _mapper = mapper;
            _cookingTimeRepository = cookingTimeRepository;
        }

        // get all cooking times
        [HttpGet]
        public IActionResult Get()
        {
            var cookingTime = _mapper.Map<List<CookingTimeDTO>>(_cookingTimeRepository.GetCookingTimes());

            if (cookingTime == null || cookingTime.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(cookingTime);
        }

        // get cooking time from id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_cookingTimeRepository.CookingTimeExists(id))
                return NotFound();

            var cookingTime = _mapper.Map<CookingTimeDTO>(_cookingTimeRepository.GetCookingTime(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(cookingTime);
        }

        // get cooking time from recipeId
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            var cookingTime = _mapper.Map<CookingTimeDTO>(_cookingTimeRepository.GetCookingTimeForRecipe(recipeId));

            if (cookingTime == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(cookingTime);
        }





        [HttpPost]
        public IActionResult CreateCookingTime([FromBody] CookingTimeDTO cookingCreate)
        {
            if (cookingCreate == null)
                return BadRequest();

            var cookingTime = _cookingTimeRepository.GetCookingTimes()
                .FirstOrDefault(c => c.Minutes == cookingCreate.Minutes);

            if (cookingTime != null)
            {
                ModelState.AddModelError("", "CookingTime Already Exist");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cookingMap = _mapper.Map<CookingTime>(cookingCreate);

            if (!_cookingTimeRepository.CreateCookingTime(cookingMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }


            return Ok();
        }


    }
}
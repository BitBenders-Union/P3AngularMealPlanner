
namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private IMapper _mapper;
        private IUnitRepository _unitRepository;

        public UnitController(IMapper mapper, IUnitRepository unitRepository)
        {
            _mapper = mapper;
            _unitRepository = unitRepository;
        }

        // get all
        [HttpGet]
        public IActionResult Get()
        {
            var units = _mapper.Map<List<UnitDTO>>(_unitRepository.GetUnits());

            if (units == null || units.Count() == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(units);
        }

        // get by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_unitRepository.UnitExists(id))
                return NotFound();

            var units = _mapper.Map<UnitDTO>(_unitRepository.GetUnitById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(units);
        }

        // get by recipe Id
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            var unit = _mapper.Map<UnitDTO>(_unitRepository.GetUnitForRecipe(recipeId));

            if (unit == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(unit);

        }

        // get unit by name
        [HttpGet("{name}")]
        public IActionResult GetById(string name)
        {
            if (!_unitRepository.UnitExists(name))
                return NotFound();

            var unit = _mapper.Map<UnitDTO>(_unitRepository.GetUnitByName(name));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(unit);
        }

        // get unit from ingredient
        [HttpGet("byIngredientId/{ingredientId}")]
        public IActionResult GetByIngredientId(int ingredientId)
        {
            var unit = _mapper.Map<UnitDTO>(_unitRepository.GetUnitFromIngredient(ingredientId));

            if (unit == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(unit);

        }





        [HttpPost]
        public IActionResult CreateUnit([FromBody] UnitDTO unitCreate)
        {
            // checks if the input form body is null
            if (unitCreate == null)
                return BadRequest();

            // looks for other quantities with the same value
            var unit = _unitRepository.GetUnits()
                .FirstOrDefault(a => a.Measurement == unitCreate.Measurement);

            // if another quantity does exist
            if (unit != null)
            {
                //TODO: logic that makes it so the created amount uses the existing amount
                ModelState.AddModelError("", "Unit Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unitMap = _mapper.Map<Unit>(unitCreate);

            // create the amount and check if it saved
            if (!_unitRepository.CreateUnit(unitMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


    }
}


namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmountController : ControllerBase // Delete this
    {
        private readonly IMapper _mapper;
        private readonly IAmountRepository _amountRepository;

        public AmountController(IMapper mapper, IAmountRepository amountRepository)
        {
            _mapper = mapper;
            _amountRepository = amountRepository;
        }

        // Get all amounts
        [HttpGet]
        public IActionResult Get()
        {
            // get the amounts from the repository method & map it to only show data in DTO format
            var amounts = _mapper.Map<List<AmountDTO>>(_amountRepository.GetAmounts());

            // check if any amounts was found
            if (amounts == null || amounts.Count() == 0)
                return NotFound("Not Found");

            // check if the model is as it should be
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // returns Ok with the list of amounts
            return Ok(amounts);
        }

        // Get amount from id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // check if amount exists from id
            if (!_amountRepository.AmountExists(id))
                return NotFound("Not Found");

            // get the amount from id
            var amount = _mapper.Map<AmountDTO>(_amountRepository.GetAmount(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(amount);
        }

        // get amount from recipeId
        // might not be very useful
        [HttpGet("byRecipe/{recipeId}")]
        public IActionResult GetAmountByRecipeId(int recipeId)
        {
            var amount = _mapper.Map<List<AmountDTO>>(_amountRepository.GetAmountsFromRecipe(recipeId));

            if (amount == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(amount);
        }

        // get amount from ingredient
        [HttpGet("byIngredient/{ingredientId}")]
        public IActionResult GetAmountByIngredientId(int ingredientId)
        {
            var amount = _mapper.Map<AmountDTO>(_amountRepository.GetAmountForIngredient(ingredientId));

            if (amount == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(amount);
        }



        [HttpPost]
        public IActionResult CreateAmount([FromBody] AmountDTO amountCreate)
        {
            // checks if the input form body is null
            if (amountCreate == null)
                return BadRequest();

            // looks for other quantities with the same value
            var amount = _amountRepository.GetAmounts()
                .FirstOrDefault(a => a.Quantity == amountCreate.Quantity);

            // if another quantity does exist
            if (amount != null)
            {
                //TODO: logic that makes it so the created amount uses the existing amount
                ModelState.AddModelError("", "Quantity Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var amountMap = _mapper.Map<Amount>(amountCreate);

            // create the amount and check if it saved
            if (!_amountRepository.CreateAmount(amountMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }


    }
}

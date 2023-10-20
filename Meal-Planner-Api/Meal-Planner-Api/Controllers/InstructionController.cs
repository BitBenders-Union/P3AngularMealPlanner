
namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructionController : ControllerBase
    {
        private IMapper _mapper;
        private IInstructionRepository _instructionRepository;

        public InstructionController(IMapper mapper, IInstructionRepository instructionRepository)
        {
            _mapper = mapper;
            _instructionRepository = instructionRepository;
        }

        // get all instructions
        [HttpGet]
        public IActionResult Get()
        {
            var instructions = _mapper.Map<List<InstructionDTO>>(_instructionRepository.GetInstructions());

            if (instructions == null || instructions.Count() == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(instructions);
        }

        // get all instructions in a recipe
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            var instructions = _mapper.Map<List<InstructionDTO>>(_instructionRepository.GetInstructionsByRecipeID(recipeId));

            if (instructions == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(instructions);

        }

        // get instruction from id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_instructionRepository.InstructionExists(id))
                return NotFound();

            var instruction = _mapper.Map<InstructionDTO>(_instructionRepository.GetInstruction(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(instruction);
        }








        [HttpPost]
        public IActionResult CreateInstruction([FromBody] InstructionDTO instructionCreate)
        {
            if (instructionCreate == null)
                return BadRequest();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var instruction = _mapper.Map<Instruction>(instructionCreate);

            if (!_instructionRepository.CreateInstruction(instruction))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return BadRequest(ModelState);
            }

            // after all checks passed return Ok
            return Ok();

        }



    }
}

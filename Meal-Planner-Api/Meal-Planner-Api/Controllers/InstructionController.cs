using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            
            if(instructions == null || instructions.Count() == 0)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(instructions);
        }

        // get all instructions in a recipe
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            var instructions = _mapper.Map<List<InstructionDTO>>(_instructionRepository.GetInstructionsByRecipeID(recipeId));

            if(instructions == null)
                return NotFound("Not Found");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(instructions);

        }

        // get instruction from id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_instructionRepository.InstructionExists(id))
                return NotFound("Not Found");

            var instruction = _mapper.Map<InstructionDTO>(_instructionRepository.GetInstruction(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(instruction);
        }



    }
}

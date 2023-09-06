using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServingsController : ControllerBase
    {
        private IMapper _mapper;
        private IServingsRepository _servingsRepository;

        public ServingsController(IMapper mapper, IServingsRepository servingsRepository)
        {
            _mapper = mapper;
            _servingsRepository = servingsRepository;
        }

        // get all servings
        [HttpGet]
        public IActionResult Get()
        {
            var serving = _mapper.Map<List<ServingsDTO>>(_servingsRepository.GetServings());

            if(serving == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(serving);
        }

        // get serving by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_servingsRepository.servingExist(id))
                return NotFound("Not Found");

            var serving = _mapper.Map<ServingsDTO>(_servingsRepository.GetServing(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(serving);
        }

        // get serving by recipe Id
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            var serving = _mapper.Map<ServingsDTO>(_servingsRepository.GetServingForRecipe(recipeId));

            if (serving == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(serving);

        }


    }
}

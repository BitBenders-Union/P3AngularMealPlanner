using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeScheduleController : ControllerBase
    {
        private IMapper _mapper;
        private IRecipeScheduleRepository _recipeScheduleRepository;

        public RecipeScheduleController(IMapper mapper, IRecipeScheduleRepository recipeScheduleRepository)
        {
            _mapper = mapper;
            _recipeScheduleRepository = recipeScheduleRepository;
        }

        // get all schedules
        [HttpGet]
        public IActionResult Get()
        {
            var schedule = _mapper.Map<List<RecipeScheduleDTO>>(_recipeScheduleRepository.GetRecipeSchedules());

            if(schedule == null || schedule.Count() == 0)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(schedule);

        }

        // get schedule from id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_recipeScheduleRepository.RecipeScheduleExists(id))
                return NotFound("Not Found");

            var schedule = _mapper.Map<RecipeScheduleDTO>(_recipeScheduleRepository.GetRecipeSchedule(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(schedule);
            
        }

        // get schedule for a user
        [HttpGet("byUserId/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var schedule = _mapper.Map<RecipeScheduleDTO>(_recipeScheduleRepository.GetRecipeScheduleForUser(userId));

            if (schedule == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(schedule);
        }


    }
}

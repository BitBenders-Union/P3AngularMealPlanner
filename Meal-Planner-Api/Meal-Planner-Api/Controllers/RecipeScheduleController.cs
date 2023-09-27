using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeScheduleController : ControllerBase
    {
        private IMapper _mapper;
        private IRecipeScheduleRepository _recipeScheduleRepository;
        private IRecipeRepository _recipeRepository;

        public RecipeScheduleController(IMapper mapper, IRecipeScheduleRepository recipeScheduleRepository, IRecipeRepository recipeRepository)
        {
            _mapper = mapper;
            _recipeScheduleRepository = recipeScheduleRepository;
            _recipeRepository = recipeRepository;
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

        [HttpPost]
        public IActionResult CreateRecipeSchedule([FromBody] RecipeScheduleDTO scheduleCreate)
        {
            if (scheduleCreate == null) 
                return BadRequest();
            
            var scheduleMap = _mapper.Map<RecipeSchedule>(scheduleCreate);

            if(!_recipeScheduleRepository.CreateRecipeSchedule(scheduleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }


            return Ok("Success");
        }


        [HttpPatch("update")]
        public IActionResult UpdateRecipeInSchedule([FromBody] RecipeScheduleDTO scheduleData)
        {
            if (scheduleData == null)
                return BadRequest();

            if (!_recipeScheduleRepository.RecipeScheduleExists(scheduleData.Id))
                return NotFound("Not Found");


            var getSchedule = _recipeScheduleRepository.GetRecipeSchedule(scheduleData.Id);

            if (!_recipeScheduleRepository.UpdateRecipeInSchedule(getSchedule, scheduleData.RecipeId))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }

    }
}

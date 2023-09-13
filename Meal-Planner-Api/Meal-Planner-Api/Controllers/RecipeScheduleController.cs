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

        [HttpPost]
        public IActionResult CreateRecipeSchedule([FromBody] RecipeScheduleDTO scheduleCreate)
        {
            if (scheduleCreate == null) 
                return BadRequest();
            
            //TODO: figure out how to implement this properly
            // the post should happen on user creation
            // it should also take the user and bind it to the schedule
            // it should also have multiple entries in the table with rows and columns, where recipes are empty.




            var scheduleMap = _mapper.Map<RecipeSchedule>(scheduleCreate);

            if(!_recipeScheduleRepository.CreateRecipeSchedule(scheduleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }


            return Ok("Success");
        }

    }
}

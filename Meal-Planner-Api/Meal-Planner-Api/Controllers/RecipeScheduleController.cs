
using Meal_Planner_Api.Models;

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


            if (schedule == null || schedule.Count() == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(schedule);

        }

        // get schedule from id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_recipeScheduleRepository.RecipeScheduleExists(id))
                return NotFound();

            var schedule = _mapper.Map<RecipeScheduleDTO>(_recipeScheduleRepository.GetRecipeSchedule(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(schedule);

        }

        // get schedule for a user
        [HttpGet("byUserId/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var schedule = _recipeScheduleRepository.GetRecipeScheduleForUser(userId);

            if (schedule == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            List<RecipeScheduleDTO> userSchedules = new();
            foreach (var sched in schedule)
            {

                RecipeScheduleDTO scheduleDTO = new RecipeScheduleDTO
                {
                    Row = sched.Row,
                    Column = sched.Column,
                    RecipeId = sched.RecipeId,
                    User = new UserOnlyNameDTO
                    {
                        Id = sched.User.Id,
                        Username = sched.User.Username
                    }

                };

                userSchedules.Add(scheduleDTO);
            }

            return Ok(userSchedules);
        }

        [HttpPost]
        public IActionResult CreateRecipeSchedule([FromBody] RecipeScheduleDTO scheduleCreate)
        {
            if (scheduleCreate == null)
                return BadRequest();

            var scheduleMap = _mapper.Map<RecipeSchedule>(scheduleCreate);

            if (!_recipeScheduleRepository.CreateRecipeSchedule(scheduleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }


            return Ok();
        }


        [HttpPatch("update")]
        public IActionResult UpdateRecipeInSchedule([FromBody] RecipeScheduleDTO scheduleData)
        {
            if (scheduleData == null)
                return BadRequest();

            var getSchedule = _recipeScheduleRepository.GetRecipeSchedule(scheduleData.User.Id, scheduleData.Row, scheduleData.Column);

            if (!_recipeScheduleRepository.UpdateRecipeInSchedule(getSchedule, scheduleData.RecipeId))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

    }
}

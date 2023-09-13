using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookingTimeController : ControllerBase
    {
        private IMapper _mapper;
        private ICookingTimeRepository _cookingTimeRepository;

        public CookingTimeController(IMapper mapper, ICookingTimeRepository cookingTimeRepository)
        {
            _mapper = mapper;
            _cookingTimeRepository = cookingTimeRepository;
        }

        // get all cookingtimes
        [HttpGet]
        public IActionResult Get()
        {
            var cookingTime = _mapper.Map<List<CookingTimeDTO>>(_cookingTimeRepository.GetCookingTimes());

            if (cookingTime == null || cookingTime.Count() == 0)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(cookingTime);
        }

        // get cookingtime from id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_cookingTimeRepository.CookingTimeExists(id))
                return NotFound("Not Found");

            var cookingTime = _mapper.Map<CookingTimeDTO>(_cookingTimeRepository.GetCookingTime(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(cookingTime);
        }

        // get cookingtime from recipeId
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            var cookingTime = _mapper.Map<CookingTimeDTO>(_cookingTimeRepository.GetCookingTimeForRecipe(recipeId));

            if(cookingTime == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(cookingTime);
        }





        [HttpPost]
        public IActionResult CreateCookingTime([FromBody] CookingTimeDTO cookingCreate)
        {
            if(cookingCreate == null)
                return BadRequest();

            var cookingTime = _cookingTimeRepository.GetCookingTimes()
                .FirstOrDefault(c => c.Minutes == cookingCreate.Minutes);

            if(cookingTime != null)
            {
                ModelState.AddModelError("", "CookingTime Already Exist");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cookingMap = _mapper.Map<CookingTime>(cookingCreate);

            if (!_cookingTimeRepository.CreateCookingTime(cookingMap))
            {
                ModelState.AddModelError("","Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }


            return Ok("Success");
        }


    }
}
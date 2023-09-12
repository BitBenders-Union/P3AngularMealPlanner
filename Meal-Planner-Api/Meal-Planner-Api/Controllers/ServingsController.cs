using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Meal_Planner_Api.Repositories;
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

            if(serving == null|| serving.Count() == 0)
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

        [HttpPost]
        public IActionResult CreateServing([FromBody] ServingsDTO servingCreate)
        {
            // checks if the input form body is null
            if (servingCreate == null)
                return BadRequest();

            // looks for other quantities with the same value
            var serving = _servingsRepository.GetServings()
                .FirstOrDefault(a => a.Quantity == servingCreate.Quantity);

            // if another quantity does exist
            if (serving != null)
            {
                //TODO: logic that makes it so the created amount uses the existing amount
                ModelState.AddModelError("", "Quantity Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //TODO: Servings map can't be found or is unsupported, fix this
            var servingMap = _mapper.Map<Servings>(servingCreate);


            // create the amount and check if it saved
            if (!_servingsRepository.CreateServing(servingMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }


    }
}

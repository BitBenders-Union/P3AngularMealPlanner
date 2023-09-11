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
    public class UnitController : ControllerBase
    {
        private IMapper _mapper;
        private IUnitRepository _unitRepository;

        public UnitController(IMapper mapper, IUnitRepository unitRepository)
        {
            _mapper = mapper;
            _unitRepository = unitRepository;
        }

        // get all
        [HttpGet]
        public IActionResult Get() 
        {
            var units = _mapper.Map<List<UnitDTO>>(_unitRepository.GetUnits());

            if(units == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(units);
        }

        // get by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_unitRepository.UnitExists(id))
                return NotFound("Not Found");

            var units = _mapper.Map<UnitDTO>(_unitRepository.GetUnitById(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(units);
        }

        // get by recipe Id
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            var unit = _mapper.Map<UnitDTO>(_unitRepository.GetUnitForRecipe(recipeId));

            if (unit == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(unit);

        }

        // get unit by name
        [HttpGet("{name}")]
        public IActionResult GetById(string name) 
        {
            if (!_unitRepository.UnitExists(name))
                return NotFound("Not Found");

            var unit = _mapper.Map<UnitDTO>(_unitRepository.GetUnitByName(name));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(unit);
        }

        // get unit from ingredient
        [HttpGet("byIngredientId/{ingredientId}")]
        public IActionResult GetByIngredientId(int ingredientId)
        {
            var unit = _mapper.Map<UnitDTO>(_unitRepository.GetUnitFromIngredient(ingredientId));

            if (unit == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(unit);

        }


    }
}

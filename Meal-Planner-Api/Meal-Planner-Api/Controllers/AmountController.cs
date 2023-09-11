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
    public class AmountController : ControllerBase
    {
        private IMapper _mapper;
        private IAmountRepository _amountRepository;

        public AmountController(IMapper mapper, IAmountRepository amountRepository)
        {
            _mapper = mapper;
            _amountRepository = amountRepository;
        }

        // Get all amounts
        [HttpGet]
        public IActionResult Get()
        {
            // get the amounts from the repository method & map it to only show data in DTO format
            var amounts = _mapper.Map<List<AmountDTO>>(_amountRepository.GetAmounts());
            
            // check if any amounts was found
            if (amounts == null || amounts.Count() == 0)
                return NotFound("Not Found");

            // check if the model is as it should be
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // returns Ok with the list of amounts
            return Ok(amounts);
        }

        // Get amount from id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // check if amount exists from id
            if(!_amountRepository.AmountExists(id))
                return NotFound("Not Found");

            // get the amount from id
            var amount = _mapper.Map<AmountDTO>(_amountRepository.GetAmount(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(amount);
        }

        // get amount from recipeId
        // might not be very useful
        [HttpGet("byRecipe/{recipeId}")]
        public IActionResult GetAmountByRecipeId(int recipeId)
        {
            var amount = _mapper.Map<AmountDTO>(_amountRepository.GetAmountsFromRecipe(recipeId));

            if (amount == null)
                return NotFound("Not Found");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(amount);
        }

        // get amount from ingredient
        [HttpGet("byIngredient/{ingredientId}")]
        public IActionResult GetAmountByIngredientId(int ingredientId)
        {
            var amount = _mapper.Map<AmountDTO>(_amountRepository.GetAmountForIngredient(ingredientId));

            if (amount == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(amount);
        }


    }
}

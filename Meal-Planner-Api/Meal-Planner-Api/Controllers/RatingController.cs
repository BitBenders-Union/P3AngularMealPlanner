using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IMapper _mapper;
        private IRatingRepository _ratingRepository;

        public RatingController(IMapper mapper, IRatingRepository ratingRepository)
        {
            _mapper = mapper;
            _ratingRepository = ratingRepository;
        }

        // get all ratings
        [HttpGet]
        public IActionResult Get()
        {
            var ratings = _mapper.Map<List<RatingDTO>>(_ratingRepository.GetRatings());
            
            if(ratings == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(ratings);
        }

        // get rating by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_ratingRepository.ratingExists(id))
                return NotFound("Not Found");
            
            var ratings = _mapper.Map<RatingDTO>(_ratingRepository.GetRating(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }


        // get ratings by recipe id
        [HttpGet("byRecipeId/{recipeId}")]
        public IActionResult GetByRecipeId(int recipeId)
        {
            if(!_ratingRepository.recipeRatingsExists(recipeId))
                return NotFound("Not Found");

            var ratings = _mapper.Map<List<RatingDTO>>(_ratingRepository.GetRatingsForRecipe(recipeId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }

        // get ratings by user
        [HttpGet("byUserId/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var ratings = _mapper.Map<List<RatingDTO>>(_ratingRepository.GetRatingsForUser(userId));

            if (ratings == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }

        // get user rating for a recipe
        [HttpGet("byUserId/{userId}/recipeId")]
        public IActionResult GetByUserId(int userId, int recipeId)
        {
            var ratings = _mapper.Map<List<RatingDTO>>(_ratingRepository.GetRecipeUserRating(userId, recipeId));

            if (ratings == null)
                return NotFound("Not Found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }
    }
}

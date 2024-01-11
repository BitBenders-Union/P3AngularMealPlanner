
namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IMapper _mapper;
        private IRatingRepository _ratingRepository;
        private IRecipeRepository _recipeRepository;
        private IUserRepository _userRepository;

        public RatingController(IMapper mapper, IRatingRepository ratingRepository, IRecipeRepository recipeRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _ratingRepository = ratingRepository;
            _recipeRepository = recipeRepository;
            _userRepository = userRepository;
        }

        // get all ratings
        [HttpGet]
        public IActionResult Get()
        {
            var ratings = _mapper.Map<List<RatingDTO>>(_ratingRepository.GetRatings());

            if (ratings == null || ratings.Count() == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(ratings);
        }

        // get rating by id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_ratingRepository.ratingExists(id))
                return NotFound();

            var ratings = _mapper.Map<RatingDTO>(_ratingRepository.GetRating(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }


        // get ratings by recipe id
        [HttpGet("byRecipeId/{userId}/{recipeId}")]
        public IActionResult GetByRecipeId(int userId, int recipeId)
        {
            if (!_ratingRepository.recipeRatingsExists(userId, recipeId))
                return NotFound();

            var ratings = _mapper.Map<List<RatingDTO>>(_ratingRepository.GetRatingsForRecipe(recipeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }

        // get ratings by user
        [HttpGet("byUserId/{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            var ratings = _mapper.Map<List<RatingDTO>>(_ratingRepository.GetRatingsForUser(userId));

            if (ratings == null)
                return NotFound();

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
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }


        [HttpGet("recipeRating/{recipeId}")]
        public IActionResult GetRecipeRating(int recipeId)
        {
            var rating = _ratingRepository.GetRecipeRating(recipeId);

            if (rating == null)
                return NotFound();

            var ratingDTO = _mapper.Map<RatingDTO>(rating);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratingDTO);
        }



        [HttpPut("upsert/{userId}/{recipeId}")]
        public IActionResult CreateRating([FromBody] RatingDTO ratingCreate, int userId, int recipeId)
        {
            if (ratingCreate == null || userId == 0 || recipeId == 0)
                return BadRequest();


            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            // if rating doesn't exist, create it

            if (!_ratingRepository.ratingExists(ratingCreate.Score))
            {
                _ratingRepository.CreateRating(_mapper.Map<Rating>(ratingCreate));
            }

            
            // if recipe rating exists, update it
            // if it doesn't exist create a new one
            if (_ratingRepository.recipeRatingsExists(userId, recipeId))
            {

                RecipeRating recipeRating = _ratingRepository.GetRecipeRating(userId, recipeId);

                recipeRating.Rating = _ratingRepository.GetRatingFromScore(ratingCreate.Score);

                _ratingRepository.UpdateRecipeRating(recipeRating);
            }
            else
            {
                RecipeRating recipeRating = new RecipeRating()
                {
                    Recipe = _recipeRepository.GetRecipe(recipeId),
                    User = _userRepository.GetUser(userId),
                    Rating = _ratingRepository.GetRatingFromScore(ratingCreate.Score)
                };

                _ratingRepository.CreateRecipeRating(recipeRating);

            }


            return Ok();
        }



    }
}

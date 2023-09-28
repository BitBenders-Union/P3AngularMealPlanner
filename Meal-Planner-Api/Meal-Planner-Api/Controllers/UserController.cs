using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Meal_Planner_Api.Repositories;
using Meal_Planner_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace Meal_Planner_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IMapper _mapper;
        private IUserRepository _userRepository;
        private IRecipeScheduleRepository _recipeScheduleRepository;
        private IHashingService _hashingService;
        private IJwtTokenService _jwtTokenService;

        public UserController(
            IMapper mapper, 
            IUserRepository userRepository, 
            IHashingService hashingService, 
            IJwtTokenService jwtTokenService,
            IRecipeScheduleRepository recipeScheduleRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _recipeScheduleRepository = recipeScheduleRepository;
            _hashingService = hashingService;
            _jwtTokenService = jwtTokenService;
        }

        // get all users
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var users = _mapper.Map<List<UserOnlyNameDTO>>(_userRepository.GetUsers());

            if(users == null || users.Count() == 0)
                return NotFound("Not Found");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);

        }

        // get user by id
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!_userRepository.UserExists(id))
                return NotFound("Not Found");

            var user = _mapper.Map<UserOnlyNameDTO>(_userRepository.GetUser(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }


        // get user by username
        [Authorize]
        [HttpGet("/username/{username}")]
        public IActionResult Get(string username)
        {
            if (!_userRepository.UserExists(username))
                return NotFound("Not Found");

            var user = _mapper.Map<UserOnlyNameDTO>(_userRepository.GetUser(username));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);

        }


        // validate user
        [HttpPost("/validate")]
        public IActionResult validateUser([FromBody] UserDTO user)
        {
            // first find user by username
            // then find salt
            // then hash password with salt
            // then compare salted hash with stored hash
            // then return Ok(true || false)

            if (user == null)
                return BadRequest();

            // check if username exist
            if (!_userRepository.UserExists(user.Username))
                return NotFound("User Not Found");

            // get user & user.passwordSalt
            var userGet = _userRepository.GetUser(user.Username);
            

            // hash the password
            byte[] hash = _hashingService.PasswordHashing(user.Password, userGet.PasswordSalt);

            // validate hash and username
            // maybe change to not use username, since we already validate username before this
            // we can't change previous methodology since password hashing requires the correct salt
            var validate = _userRepository.ValidateUser(hash, user.Username);
            if(!validate)
                return BadRequest();

            userGet.Token = _jwtTokenService.CreateJwtToken(userGet);
            var newAccessToken = userGet.Token;
            var newRefreshToken = _jwtTokenService.CreateRefreshToken();
            userGet.RefreshToken = newRefreshToken;
            userGet.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            _userRepository.Save();

            return Ok(new TokenDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,

            });
        }



        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO user)
        {
            // checks if the input form body is null
            if (user.Username.IsNullOrEmpty() || user.Password.IsNullOrEmpty())
                return BadRequest();

            // looks for other users with the same value
            var userExist = _userRepository.UserExists(user.Username);

            // if user exist
            if (userExist)
            {
                ModelState.AddModelError("", "User Already Exists");
                return StatusCode(422, ModelState);
            }

            // create a new user
            byte[] salt = _hashingService.GenerateSalt();
            byte[] hash = _hashingService.PasswordHashing(user.Password, salt);

            User newUser = new User
            {
                Username = user.Username,
                PasswordSalt = salt,
                PasswordHash = hash
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            // create the user and check if it saved
            if (!_userRepository.CreateUser(newUser))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            var userGet = _userRepository.GetUser(newUser.Username);

            // after creating a user create a recipe-schedule for that user
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    RecipeSchedule recipeSchedule = new RecipeSchedule
                    {
                        Row = i,
                        Column = j,
                        User = userGet,
                        RecipeId = null
                    };
                    _recipeScheduleRepository.CreateRecipeSchedule(recipeSchedule);
                }
            }


            return Ok("Success");
        }

        [HttpPost("/refresh")]
        public IActionResult Refresh(TokenDTO tokenDTO)
        {
            if(tokenDTO == null)
                return BadRequest("Invalid client request");
            string accessToken = tokenDTO.AccessToken;
            string refreshToken = tokenDTO.RefreshToken;
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = _userRepository.GetUser(username);
            if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return BadRequest("Invalid client request");
            var newAccessToken = _jwtTokenService.CreateJwtToken(user);
            var newRefreshToken = _jwtTokenService.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _userRepository.Save();
            return Ok(new TokenDTO()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
            

        }

        // update user
        [Authorize]
        [HttpPut("update/{userId}")]
        public IActionResult UpdateUser([FromBody] UserDTO user, int userId)
        {
            // validate body
            if (user == null)
                return BadRequest();

            // validate user exist
            if (!_userRepository.UserExists(userId))
                return NotFound("User Not Found");

            // validate user
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // hash password

            // get new salt for new password
            byte[] salt = _hashingService.GenerateSalt();
            byte[] hash = _hashingService.PasswordHashing(user.Password, salt);


            // update user
            User updateUser = new User
            {
                Id = userId,
                Username = _userRepository.GetUser(userId).Username,
                PasswordSalt = salt,
                PasswordHash = hash
            };


            if (!_userRepository.UpdateUser(updateUser))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }


        [Authorize]
        [HttpDelete("delete/{userId}")]
        public IActionResult DeleteUser([FromBody] UserDTO user, int userId)
        {

            // validate body
            if (user == null)
                return BadRequest();

            // validate user exist
            if (!_userRepository.UserExists(userId))
                return NotFound("User Not Found");

            // validate user
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // login to the user you are trying to delete

            var existingUser = _userRepository.GetUser(userId);
            var hash = _hashingService.PasswordHashing(user.Password, existingUser.PasswordSalt);
            if (hash != existingUser.PasswordHash)
            {
                ModelState.AddModelError("", "Username does not match");
                return StatusCode(500, ModelState);
            }

            // delete user
            if(!_userRepository.DeleteUser(_userRepository.GetUser(userId)))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return Ok("User Deleted");
        }



    }
}
using AutoMapper;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Meal_Planner_Api.Repositories;
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
        private IHashingService _hashingService;

        public UserController(IMapper mapper, IUserRepository userRepository, IHashingService hashingService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _hashingService = hashingService;
        }

        // get all users
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

            return Ok(validate);

        }



        [HttpPost]
        public IActionResult CreateUnit([FromBody] UserDTO user)
        {
            // checks if the input form body is null
            if (user.Username.IsNullOrEmpty() || user.Password.IsNullOrEmpty())
                return BadRequest();

            // looks for other users with the same value
            var userGet = _userRepository.GetUsers()
                .FirstOrDefault(a => a.Username.Trim().ToUpper() == user.Username.Trim().ToUpper());

            

            // if another quantity does exist
            if (userGet != null)
            {
                //TODO: logic that makes it so the created amount uses the existing amount
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


            // create the amount and check if it saved
            if (!_userRepository.CreateUser(newUser))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }


    }
}
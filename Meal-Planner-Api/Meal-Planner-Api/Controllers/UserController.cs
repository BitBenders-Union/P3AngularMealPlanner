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
            var users = _mapper.Map<List<UserDTO>>(_userRepository.GetUsers());

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

            var user = _mapper.Map<UserDTO>(_userRepository.GetUser(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }


        // get user by username
        [HttpGet("{username}")]
        public IActionResult Get(string username)
        {
            if (!_userRepository.UserExists(username))
                return NotFound("Not Found");

            var user = _mapper.Map<UserDTO>(_userRepository.GetUser(username));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);

        }


        // validate user
        [HttpGet("{password}/{username}")]
        public IActionResult validateUser(string password, string username)
        {
            // first find user by username
            // then find salt
            // then hash password with salt
            // then compare salted hash with stored hash
            // then return Ok(true || false)

            // check if username exist
            if (!_userRepository.UserExists(username))
                return NotFound("User Not Found");

            // get user & user.passwordSalt
            var user = _userRepository.GetUser(username);

            // hash the password
            byte[] hash = _hashingService.PasswordHashing(password, user.PasswordSalt);

            // validate hash and username
            // maybe change to not use username, since we already validate username before this
            // we can't change previous methodology since password hashing requires the correct salt
            var validate = _userRepository.ValidateUser(hash, username);

            return Ok(validate);

        }



        [HttpPost]
        public IActionResult CreateUnit([FromBody] UserDTO userCreate)
        {
            // checks if the input form body is null
            if (userCreate == null)
                return BadRequest();

            // looks for other quantities with the same value
            var user = _userRepository.GetUsers()
                .FirstOrDefault(a => a.Username.Trim().ToUpper == userCreate.Username.Trim().ToUpper());

            

            // if another quantity does exist
            if (unit != null)
            {
                //TODO: logic that makes it so the created amount uses the existing amount
                ModelState.AddModelError("", "Unit Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unitMap = _mapper.Map<Unit>(userCreate);

            // create the amount and check if it saved
            if (!_unitRepository.CrateUnit(unitMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }


    }
}

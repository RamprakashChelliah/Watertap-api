using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WaterTap.Api.Data;
using WaterTap.Api.Models;
using WaterTap.Api.RepositoryServices;

namespace WaterTap.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        public readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            var users = _userRepository.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("{emailId}")]
        public IActionResult GetUser(string emailId)
        {
            var users = _userRepository.GetUser(emailId);

            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddNewUser([FromBody] UserModel userModel)
        {
            _userRepository.AddNewUser(userModel);

            return Ok(userModel.EmailId);
        }

        [HttpPut("{emailId}")]
        public IActionResult UpdateUser(string emailId, [FromBody] UserModel userModel)
        {
            _userRepository.UpdateUser(emailId, userModel);

            return Ok(userModel.EmailId);
        }
    }
}

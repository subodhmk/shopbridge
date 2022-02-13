using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopbridge_base.Models;
using Shopbridge_base.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   

namespace Shopbridge_base.Controllers
{
    [Authorize]
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Authentication model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] Authentication model)
        {
            var userUID = _userRepo.IsUniqueUser(model.Username);
            if (!userUID)
            {
                return BadRequest(new { message = "User Already Exists" });
            }

            var user = _userRepo.Register(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Error Occured In Registration" });
            }

            return Ok("User Created Sucessfully");
        }
    }
}

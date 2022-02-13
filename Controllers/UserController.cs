using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserController> logger;

        public UserController(IUserRepository userRepository,ILogger<UserController> _logger)
        {
            _userRepo = userRepository;
            logger = _logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Authentication model)
        {
            try
            {
                var user = _userRepo.Authenticate(model.Username, model.Password);
                if (user == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }
                return Ok(user);
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return BadRequest(new { message = "Error Occured In Authentication" });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] Authentication model)
        {
            try
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
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return BadRequest(new { message = "Error Occured In Registration" });
            }
        }
    }
}

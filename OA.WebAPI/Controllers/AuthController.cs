using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OA_DataAccess;
using OA_Service;

namespace OA.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepo _repo;

        public AuthController(IUserRepo repo)
        {
            _repo = repo;
        }
        /// <summary>
        /// Signup api to create user
        /// </summary>
        [HttpPost("signup")]
        public async  Task<IActionResult> Signup([FromBody]Signup signup)
        {
            var result = await _repo.SignUpAsync(signup);
            if(result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Login api to get JWT barrer token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]Login login)
        {
            var result = await _repo.LoginAsync(login);
            if (!string.IsNullOrEmpty(result))
                return Ok(result);
            else
                return Unauthorized();
        }

    }
}
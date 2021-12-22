using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OA_DataAccess;
using OA_Service;

namespace OA.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepo _repo;
       
        //Test
        public AuthController(IUserRepo repo)
        {
            _repo = repo;
        }
        /// <summary>
        /// Signup api to create user
        /// </summary>
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody]Signup signup)
        {
            var result = await _repo.SignUpAsync(signup);
            if (result.Succeeded)
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
            if (result != null)
                return Ok(result);
            else
                return Unauthorized();
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshRequest refreshRequest)
        {
            if(refreshRequest==null)
            {
                return BadRequest("Invalid request");
            }
            var result = await _repo.GetAndGenAccessTokenAndRefreshTokenFromExsting(refreshRequest);
            if(result!=null)
                return Ok(result);
            else 
                return BadRequest("Invalid request");
        }

        

    }
}
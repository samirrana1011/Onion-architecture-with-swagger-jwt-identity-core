using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OA_DataAccess;
using OA_Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OA_Service
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _siginManager;
        private readonly IConfiguration _configuration;
        public UserRepo(UserManager<User> userManager,
            SignInManager<User> siginManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _siginManager = siginManager;
            _configuration = configuration;
        }
        public async Task<IdentityResult> SignUpAsync(Signup signup)
        {
            var user = new User()
            {
                FName = signup.Name,
                Email = signup.Email,
                UserName = signup.Email,
                //CreatedDate = DateTime.Now
            };
          return  await _userManager.CreateAsync(user,signup.Password);
        }

        public async Task<string> LoginAsync(Login s)
        {
            var result = await _siginManager.PasswordSignInAsync(s.Email, s.Password, false,false);
            if(!result.Succeeded)
            {
                return null;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,s.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidUser"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
                );
          return  new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

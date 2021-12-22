using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OA_DataAccess;
using OA_Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public async Task<RefreshRequest> LoginAsync(Login s)
        {
            var result = await _siginManager.PasswordSignInAsync(s.Email, s.Password, false,false);
            if(!result.Succeeded)
            {
                return null;
            }
            User usr =await _userManager.FindByEmailAsync(s.Email);
            RefreshRequest RefreshRequest = GenerateToken(usr);
            usr.RefreshToken = RefreshRequest.RefreshToken;
            await _userManager.UpdateAsync(usr);
            return RefreshRequest;
        }

        
        public RefreshRequest GenerateToken(User usr)
        {
            var authClaims = new List<Claim>();

            if (usr != null)
            {
                authClaims.Add(new Claim(ClaimTypes.Sid, usr.Id));
                authClaims.Add(new Claim(ClaimTypes.Email, usr.Email));
                authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            }
            var appSecret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsDictionary = new Dictionary<string, object>();

            

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Claims = claimsDictionary,
                Issuer = _configuration["JWT:ValidUser"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(100),
                SigningCredentials =
                    new SigningCredentials(
                       appSecret,
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);


            var RefreshRequest = new RefreshRequest
            {
              AccessToken =  tokenHandler.WriteToken(token),
              RefreshToken =  GenerateRefreshToken()
            };
            return RefreshRequest;
        }

        public async Task<RefreshRequest>  GetAndGenAccessTokenAndRefreshTokenFromExsting(RefreshRequest refreshRequest)
        {
            try
            {
                RefreshRequest RefreshRequest = null;
                var tokenValidationParamters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidIssuer = _configuration["JWT:ValidUser"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                    ValidateLifetime = false
            };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(refreshRequest.AccessToken, tokenValidationParamters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token!");
                }
                var userId = principal.FindFirst(ClaimTypes.Sid)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    throw new SecurityTokenException($"Missing claim: {ClaimTypes.Sid}!");
                }
                User usr = await _userManager.FindByIdAsync(userId);
                if (usr.RefreshToken == refreshRequest.RefreshToken)
                {
                    RefreshRequest = GenerateToken(usr);
                    usr.RefreshToken = RefreshRequest.RefreshToken;
                    await _userManager.UpdateAsync(usr);
                }
                    return RefreshRequest;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

        

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}

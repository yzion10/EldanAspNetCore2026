using ApiLesson6.DTO;
using ApiLesson6.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiLesson6.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly List<User> _users = new List<User>
        {
            new User { FirstName = "Admin", LastName = "User", UserName = "admin", Password = "admin" },
            new User { FirstName = "yosi", LastName = "Doe", UserName = "yosi", Password = "1234" }
        };

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public ActionResult<string> Login(LoginBodyDTO loginBody)
        {
            var user = ValidateUser(loginBody.UserName, loginBody.Password);

            if (user == null)
                return Unauthorized();

            // get the secret key from configuration
            var configKey = _configuration["Authentication:SecretKey"];

            // create a symmetric security key using the secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));

            // create signing credentials using the security key and the HMAC-SHA256 algorithm
            var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // create claims for the user, including their username, first name, last name, role, and password
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                new Claim("password", user.Password)
            };

            // create a JWT token with the claims, an expiration time of 30 minutes, and the signing credentials
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCreds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(tokenString);
        }

        private User ValidateUser(string userName, string password)
        {
            return _users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }
    }
}

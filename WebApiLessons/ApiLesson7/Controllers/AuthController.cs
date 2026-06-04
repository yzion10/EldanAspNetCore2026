using ApiLesson7.DTO;
using ApiLesson7.Entities;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiLesson7.Controllers
{
    //[ApiVersion(2)] // הגדרת גרסת API עבור הקונטרולר הזה לגרסה 2
    [ApiController]
    //[Route("api/auth")]
    [Route("api/v{version:apiVersion}/auth")] // הגדרת גרסת API בנתיב של הקונטרולר
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class AuthController : ControllerBase
    {
        private readonly List<User> _users = new List<User>
        {
            new User { FirstName = "Admin", LastName = "User", UserName = "admin", Password = "admin", IsAdmin = true },
            new User { FirstName = "yosi", LastName = "Doe", UserName = "yosi", Password = "1234" }
        };

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("something")]
        [MapToApiVersion(1)] // מיפוי הפעולה הזו לגרסה 1 של ה-API
        public ActionResult<string> GetSomething()
        {
            return Ok("This is something");
        }

        [HttpGet("something")]
        [MapToApiVersion(2)] // מיפוי הפעולה הזו לגרסה 2 של ה-API
        public ActionResult<string> GetSomething2()
        {
            return Ok("This is something for version 2");
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey) ?? throw new InvalidOperationException("SecretKey is not configured"));

            // create signing credentials using the security key and the HMAC-SHA256 algorithm
            var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // create claims for the user, including their username, first name, last name, role, and password
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Authentication:Issuer"] ?? throw new InvalidOperationException("Issuer is not configured")),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Authentication:Audience"] ?? throw new InvalidOperationException("Audience is not configured")),
                new Claim("IsAdmin", user.IsAdmin ? "Admin" : "User"),
                //new Claim("password", user.Password) // לא לבצע את זה. זה רק להדגמה
            };

            // create a JWT token with the claims, an expiration time of 30 minutes, and the signing credentials
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // הגדרת זמן תפוגה של הטוקן ל-30 דקות
                signingCredentials: signingCreds

                // אם רוצים לנהל טוקן ללא
                // לוגין אלא טוקן בנפרד שאני מספק ללקוח עם טווח תפוגה של ממתי עד מתי הטוקן תקף אלו הפקודות:
                //expires: new DateTime(2027, 01, 20), // הגדרת זמן תפוגה של הטוקן לתאריך מסוים
                //notBefore: new DateTime(2027, 01, 10) // הגדרת זמן התחלה של הטוקן לתאריך מסוים
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

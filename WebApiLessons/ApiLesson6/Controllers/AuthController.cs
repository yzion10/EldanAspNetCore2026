using ApiLesson6.DTO;
using ApiLesson6.Entities;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("login")]
        public ActionResult<string> Login(LoginBodyDTO loginBody)
        {
            var user = ValidateUser(loginBody.UserName, loginBody.Password);

            if (user != null)
                return Ok(user);
            else
                return Unauthorized();
        }

        private User ValidateUser(string userName, string password)
        {
            return _users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }
    }
}

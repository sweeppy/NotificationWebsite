using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.Data;
using NotificationWebsite.Dtos;
using NotificationWebsite.Models;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;

        public AuthController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Mail = dto.Mail,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)//Password hash
            };
            try
            {
                _repository.Create(user);
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
    }
}
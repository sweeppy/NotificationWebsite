using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;
using NotificationWebsite.Models.Dtos;
using NotificationWebsite.Utility.Helpers;

namespace NotificationWebsite.Controllers.API
{
    [Route("api/AuthAPI")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ApplicationDbContext _db;
        private readonly IJwtService _jwtService;

        public AuthAPIController(IUserRepository repository, ApplicationDbContext db, IJwtService jwtService)
        {
            _repository = repository;
            _db = db;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register(RegisterDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Empty fields" });//user fields empty

            if (!ModelState.IsValid) return BadRequest(ModelState);//Incorrect input

            if (_db.Users.FirstOrDefault(u => u.Email != null && u.Email.ToLower() == dto.Email.ToLower()) != null)//This user is already in the DB
            {
                ModelState.AddModelError("SameEmail", "This user is already registered");
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)//Password hash
            };
            return Created("success", _repository.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            User user = _repository.GetByEmail(dto.Email);

            if (user == null) return BadRequest(new { message = "Empty fields" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return BadRequest(new { message = "Wrong password" });

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true         //!PROTECT  (to give access only for BackEnd(not for FrontEnd))
            });

            return Ok(new
            {
                message = "success"
            });
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                if (jwt == null) return Unauthorized();

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _repository.GetById(userId);

                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
    }
}
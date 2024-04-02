using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<AuthAPIController> _logger;
        private readonly IUserRepository _repository;
        private readonly ApplicationDbContext _db;
        private readonly IJwtService _jwtService;


        public AuthAPIController(IUserRepository repository, ApplicationDbContext db, IJwtService jwtService, ILogger<AuthAPIController> logger)
        {
            _logger = logger;
            _repository = repository;
            _db = db;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register(RegisterDto dto)
        {
            try
            {
                if (dto == null) return BadRequest(new ResponseInformation()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ResponseMessage = "Not all fields are filled in"
                });//user fields empty

                if (!ModelState.IsValid) return BadRequest(new ResponseInformation()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ResponseMessage = "Incorrect input"
                });//Incorrect input

                if (_db.Users.FirstOrDefault(u =>
                                            u.Email != null &&
                                            dto.Email != null &&
                                            u.Email.ToLower() == dto.Email.ToLower()) != null)//This user is already in the DB
                {
                    return BadRequest(new ResponseInformation()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ResponseMessage = "User with this email is already registered"
                    });
                }

                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)//Password hash
                };
                return Created("success", _repository.Create(user));
            }
            catch (Exception e)
            {
                _logger.LogError($"Register error: {e.Message}");
                return BadRequest(new ResponseInformation()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ResponseMessage = "Something went wrong"
                });
            }

        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(LoginDto dto)
        {

            User user = _repository.GetByEmail(dto.Email);

            if (user == null || user.Email == null || user.Username == null || user.Password == null)
            {
                return BadRequest(new ResponseInformation()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ResponseMessage = "Wrong email"
                });
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return BadRequest(new ResponseInformation()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ResponseMessage = "Wrong password"
            });//important that key is "message"

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true         //!PROTECT  (to give access only for BackEnd(not for FrontEnd))
            });

            return Ok(new ResponseInformation()
            {
                StatusCode = StatusCodes.Status200OK,
                ResponseMessage = "Welcome to our site!"
            });
        }

        [HttpGet("user"), Authorize]
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
                return
                Unauthorized();
            }
        }
    }
}
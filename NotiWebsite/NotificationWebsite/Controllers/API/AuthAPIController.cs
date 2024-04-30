using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;
using NotificationWebsite.Models.Dtos;
using NotificationWebsite.Models.Models.Dtos;
using NotificationWebsite.Utility.Configuration.Jwt;
using NotificationWebsite.Utility.Helpers.Jwt;

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

        private readonly JwtConfig _jwtConfig;


        public AuthAPIController(IUserRepository repository, ApplicationDbContext db,
         IJwtService jwtService, ILogger<AuthAPIController> logger,
         IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _logger = logger;
            _repository = repository;
            _db = db;
            _jwtService = jwtService;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Register(RegisterDto dto)
        {
            try
            {
                if (dto == null) return BadRequest(new RegisterRequestResponse()
                {
                    Result = false,
                    Message = "Not all fields are filled in"
                });//user fields empty

                if (!ModelState.IsValid) return BadRequest(new RegisterRequestResponse()
                {
                    Result = false,
                    Message = "Incorrect input"
                });//Incorrect input

                if (_db.Users.FirstOrDefault(u =>
                                            u.Email != null &&
                                            dto.Email != null &&
                                            u.Email.ToLower() == dto.Email.ToLower()) != null)//This user is already in the DB
                {
                    return BadRequest(new RegisterRequestResponse()
                    {
                        Result = false,
                        Message = "User with this email is already registered"
                    });
                }

                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)//Password hash
                };
                _repository.Create(user);
                var token = _jwtService.Generate(user.Id);
                return Ok(new RegisterRequestResponse()
                {
                    Result = true,
                    Token = token,
                    Message = "Successful registration!"
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Register error: {e.Message}");
                return BadRequest(new RegisterRequestResponse()
                {
                    Result = false,
                    Message = "Something went wrong"
                });
            }

        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            User user = await _repository.GetByEmail(dto.Email);

            if (user == null || user.Email == null || user.Username == null || user.Password == null)
            {
                return BadRequest(new LoginRequestResponse()
                {
                    Result = false,
                    Message = "Wrong email"
                });
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return BadRequest(new LoginRequestResponse()
            {
                Result = false,
                Message = "Wrong password"
            });

            var token = _jwtService.Generate(user.Id);

            return Ok(new LoginRequestResponse()
            {
                Result = true,
                Token = token,
                Message = "Welcome to our site!"
            });
        }
    }
}
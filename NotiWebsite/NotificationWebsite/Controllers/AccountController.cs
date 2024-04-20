using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NotificationWebsite.Models;
using NotificationWebsite.Models.Dtos;
using NotificationWebsite.Models.Models.Dtos;
using NotificationWebsite.Utility.Helpers.Validation;


namespace NotificationWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly HttpClient _httpClient;
        private readonly ILoginValidation _loginValidation;

        public AccountController(ILogger<AccountController> logger, HttpClient httpClient, ILoginValidation loginValidation)
        {
            _logger = logger;
            _httpClient = httpClient;
            _loginValidation = loginValidation;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            RegisterDto dto = new RegisterDto
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            };
            var jsonUser = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            string url = "http://localhost:5019/api/AuthAPI/register";

            var response = await _httpClient.PostAsync(url, content);

            var data = await response.Content.ReadAsStringAsync();
            var registerResponse = JsonConvert.DeserializeObject<RegisterRequestResponse>(data);
            if (registerResponse.Result)
            {
                _logger.LogInformation(registerResponse.Message);
                //Login with the same values
                return await Login(user);
            }
            else
            {
                // Handle registration error
                ModelState.AddModelError("RegistrationError", registerResponse.Message); // Add error message to model state
                TempData["ErrorMessage"] = registerResponse.Message;
                return View();
            }
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!_loginValidation.IsLoginValid(user))
            {
                return View();
            }
            LoginDto dto = new LoginDto
            {
                Email = user.Email,
                Password = user.Password
            };

            var jsonUser = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            string url = "http://localhost:5019/api/AuthAPI/login";

            var response = await _httpClient.PostAsync(url, content);

            var data = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginRequestResponse>(data);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = loginResponse.Message;

                HttpContext.Response.Cookies
                    .Append("L_Cookie", loginResponse.Token, new CookieOptions { HttpOnly = true });//add token to cookies

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = $"{loginResponse.Message}";

                return View("Login");
            }

        }
    }
}

using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models.Dtos;


namespace NotificationWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly HttpClient _httpClient;

        public AccountController(ILogger<AccountController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SignUp(RegisterDto dto)
        {
            var jsonUser = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            string url = "http://localhost:5019/api/AuthAPI/register";

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                //Login with the same values
                LoginDto loginDto = new LoginDto()
                {
                    Email = dto.Email,
                    Password = dto.Password
                };
                return await Login(loginDto);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonConvert.DeserializeObject<ResponseInformation>(error);
                // Handle registration error
                ModelState.AddModelError("RegistrationError", errorMessage.ResponseMessage); // Add error message to model state
                TempData["ErrorMessage"] = errorMessage.ResponseMessage;
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            var jsonUser = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            string url = "http://localhost:5019/api/AuthAPI/login";

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                TempData["SuccessMessage"] = "Welcome to our site!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonConvert.DeserializeObject<ResponseInformation>(error);
                TempData["ErrorMessage"] = $"{errorMessage.ResponseMessage}";

                return View("Login");
            }

        }
    }
}

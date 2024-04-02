using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;
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

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                //Login with the same values
                return await Login(user);
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            LoginDto dto = new LoginDto
            {
                Email = user.Email,
                Password = user.Password
            };
            /*if (User.Identity.IsAuthenticated)
            {
                TempData["InfoMessage"] = "You are already logged in.";
                return RedirectToAction("Index", "Home");
            }*/

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

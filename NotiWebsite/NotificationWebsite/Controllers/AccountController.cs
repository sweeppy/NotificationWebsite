using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> SignUp(RegisterDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string url = "http://localhost:5019/api/AuthAPI/register";

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                // Handle successful registration response
                return RedirectToAction("Login");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                // Handle registration error
                ModelState.AddModelError(string.Empty, errorMessage); // Add error message to model state
                return View(dto);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.Models;
using NotificationWebsite.Utility.Helpers.Jwt;

namespace NotificationWebsite.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;

        private readonly IJwtService _jwtService;
        public MainController(ILogger<MainController> logger, IJwtService jwtService)
        {
            _logger = logger;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> Home([FromServices] IHttpContextAccessor accessor)
        {
            string token = accessor.HttpContext.Request.Cookies["L_Cookie"];
            User currentUser = await _jwtService.GetUserByToken(token);
            return View(currentUser);
        }

    }
}
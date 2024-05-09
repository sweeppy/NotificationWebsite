using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;

namespace NotificationWebsite.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly IUserRepository _user_rep;
        public MainController(ILogger<MainController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _user_rep = userRepository;
        }

        public async Task<IActionResult> Home()
        {
            /*for test*/
            User user = await _user_rep.GetById(1001);
            return View(user);
        }

    }
}
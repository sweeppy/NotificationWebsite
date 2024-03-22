using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.Data;
using NotificationWebsite.Models;

namespace NotificationWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UsersDbContext _db;

        public AccountController(ILogger<AccountController> logger, UsersDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        [HttpPost]
        public IActionResult SignUp([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            _db.Users.Add(entity: user);
            _db.SaveChanges();
            return Json(new { success = true });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}

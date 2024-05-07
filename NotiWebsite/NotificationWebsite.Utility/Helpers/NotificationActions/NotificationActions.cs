using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;
using NotificationWebsite.Utility.Helpers.Jwt;

namespace NotificationWebsite.Utility.Helpers.NotificationActions
{
    public class NotificationActions : INotificationActions
    {

        private readonly ApplicationDbContext _db;
        private readonly IJwtService _jwtService;

        public NotificationActions(IJwtService jwtService, ApplicationDbContext db)
        {
            _jwtService = jwtService;
            _db = db;
        }

        public async Task<IActionResult> CreateNotification([FromBody] DateTime dateTimeParam,
         string header, string message, string social, HttpContext context)
             {
                string token = context.Request.Cookies["L_Cookie"];

                User authenticatedUser = await _jwtService.GetUserByToken(token);

                authenticatedUser.Notifications.Add(new Notification {
                    Header = header,
                    Message = message,
                    Status = "Planned",
                    SocialNetwork = social,
                    User = authenticatedUser,
                    Date = dateTimeParam
                });
                await _db.SaveChangesAsync();

                return new ObjectResult("ok");
             }
    }
}
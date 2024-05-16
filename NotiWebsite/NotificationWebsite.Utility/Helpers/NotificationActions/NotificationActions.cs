using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.DataAccess.Contracts;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;
using NotificationWebsite.Utility.Jwt;

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

        public async Task<ActionResult<string>> CreateNotification([FromBody]CreateNotificationRequest request, HttpContext context)
             {
                string token = context.Request.Cookies["L_Cookie"];

                User authenticatedUser = await _jwtService.GetUserByToken(token);

                if (authenticatedUser != null)
                {
                        authenticatedUser.Notifications.Add(new Notification {
                        Header = request.header,
                        Message = request.message,
                        Status = "Planned",
                        SocialNetwork = request.social,
                        User = authenticatedUser,
                        Date = request.dateTimeParam
                    });
                    await _db.SaveChangesAsync();

                    return ("success");
                }
                return ("error");
                
             }
    }
}
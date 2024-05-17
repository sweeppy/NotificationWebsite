using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<IActionResult> AddNotificationToDBAsync(Notification notification, User authenticatedUser)
        {
            if (authenticatedUser == null || notification == null)
            {
                return new BadRequestObjectResult("Null reference");
            }

            authenticatedUser.Notifications.Add(notification);
            await _db.SaveChangesAsync();

            return new OkObjectResult("Notification was created");
        }

        public Notification MakeNotificationFromRequest(CreateNotificationRequest request, User authenticatedUser)
        {
            Notification notification = new Notification
                {
                    Header = request.header,
                    Message = request.message,
                    SocialNetwork = request.social,
                    Date = request.dateTimeParam,
                    Status = "planned",
                    UserId = authenticatedUser.Id
                };
            return notification;
        }
    }
}
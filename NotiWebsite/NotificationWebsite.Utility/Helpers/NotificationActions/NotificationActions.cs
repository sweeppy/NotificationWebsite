using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationWebsite.DataAccess.Contracts;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;

namespace NotificationWebsite.Utility.Helpers.NotificationActions
{
    public class NotificationActions : INotificationActions
    {

        private readonly ApplicationDbContext _db;

        public NotificationActions(ApplicationDbContext db)
        {
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

        public async Task UpdateNotificationStatusAsync(Notification notification, User authenticatedUser)
        {
            if(authenticatedUser != null && notification != null)
            {
                User user = await _db.Users.Include(u => u.Notifications).FirstOrDefaultAsync(u => u.Id == authenticatedUser.Id);
                user.Notifications.FirstOrDefault(n => n.Id == notification.Id).Status = NotificationStatuses.Sent;
                await _db.SaveChangesAsync();
            }
        }
    }
}
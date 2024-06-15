using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationWebsite.Models.Contracts;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;

namespace NotificationWebsite.Utility.Helpers.NotificationActions
{
    public class NotificationActions : INotificationActions
    {

        private readonly ApplicationDbContext _db;

        private readonly GmailService _gmailService;

        public NotificationActions(ApplicationDbContext db, GmailService gmailService)
        {
            _db = db;
            _gmailService = gmailService;
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

        public async Task SendLetterAndUpdateNotificationStatusAsync(Notification notification,
         User authenticatedUser, Message message)
        {
            if(authenticatedUser != null && notification != null && message != null)
            {
                _gmailService.Users.Messages.Send(message, "me").ExecuteAsync();
                User user = await _db.Users.Include(u => u.Notifications).FirstOrDefaultAsync(u => u.Id == authenticatedUser.Id);
                user.Notifications.FirstOrDefault(n => n.Id == notification.Id).Status = NotificationStatuses.Sent;
                await _db.SaveChangesAsync();
            }
        }
    }
}
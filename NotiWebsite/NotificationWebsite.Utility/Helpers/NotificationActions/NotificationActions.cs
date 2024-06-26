using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationWebsite.Models.Contracts;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;
using Telegram.Bot;
using Microsoft.AspNetCore.Identity;

namespace NotificationWebsite.Utility.Helpers.NotificationActions
{
    public class NotificationActions : INotificationActions
    {

        private readonly ApplicationDbContext _db;

        private readonly GmailService _gmailService;

        private readonly ITelegramBotClient _telegramBotClient;

        public NotificationActions(ApplicationDbContext db, GmailService gmailService, ITelegramBotClient telegramBotClient)
        {
            _db = db;
            _gmailService = gmailService;
            _telegramBotClient = telegramBotClient;
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

        public async Task SendAndUpdateNotificationGmail(Notification notification,
         User authenticatedUser, Message message)
        {
            if(authenticatedUser != null && notification != null && message != null)
            {
                await _gmailService.Users.Messages.Send(message, "me").ExecuteAsync();

                await UpdateNotificationStatusAsunc(authenticatedUser, notification);
            }
        }

        public async Task SendAndUpdateNotificationTelegram(Telegram.Bot.Types.ChatId chatId, User user,
            Notification notification)
        {
            await _telegramBotClient.SendTextMessageAsync(chatId, $"{notification.Header.ToUpper()} \n{notification.Message}");
            await UpdateNotificationStatusAsunc(user, notification);
        }

        public async Task UpdateNotificationStatusAsunc(User authenticatedUser, Notification notification)
        {
            User user = await _db.Users.Include(u => u.Notifications).FirstOrDefaultAsync(u => u.Id == authenticatedUser.Id);
                user.Notifications.FirstOrDefault(n => n.Id == notification.Id).Status = NotificationStatuses.Sent;
                await _db.SaveChangesAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            var notification = await _db.Notifications.FirstOrDefaultAsync<Notification>(n => n.Id == id);
            return notification;
        }

        public async Task DeleteNotifcationAsync(Notification notification)
        {
            if (notification == null) return;
            _db.Notifications.Remove(notification);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateUsersDbAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
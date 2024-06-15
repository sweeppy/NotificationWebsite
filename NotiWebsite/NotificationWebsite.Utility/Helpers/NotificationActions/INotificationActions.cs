using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.Models.Contracts;
using NotificationWebsite.Models;

namespace NotificationWebsite.Utility.Helpers.NotificationActions
{
    public interface INotificationActions
    {
        public Task<IActionResult> AddNotificationToDBAsync(Notification notification, User authenticatedUser);

        public Notification MakeNotificationFromRequest(CreateNotificationRequest request, User authUser);

        public Task SendAndUpdateNotificationGmail(Notification notification, User authenticatedUser, Message message);
        
    }
}
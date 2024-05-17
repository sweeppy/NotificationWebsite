using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.DataAccess.Contracts;
using NotificationWebsite.Models;

namespace NotificationWebsite.Utility.Helpers.NotificationActions
{
    public interface INotificationActions
    {
        public Task<IActionResult> AddNotificationToDBAsync(Notification notification, User authenticatedUser);

        public Notification MakeNotificationFromRequest(CreateNotificationRequest request, User authUser);

        public Task UpdateNotificationStatusAsync(Notification notification, User authenticatedUser);
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.DataAccess.Contracts;

namespace NotificationWebsite.Utility.Helpers.NotificationActions
{
    public interface INotificationActions
    {
        public Task<ActionResult<string>> CreateNotification(
            [FromBody]CreateNotificationRequest request, HttpContext context);
    }
}
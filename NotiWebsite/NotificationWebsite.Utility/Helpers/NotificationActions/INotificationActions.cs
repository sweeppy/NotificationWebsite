using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotificationWebsite.Utility.Helpers.NotificationActions
{
    public interface INotificationActions
    {
        public Task<IActionResult> CreateNotification(
            [FromBody] DateTime dateTimeParam, string header,
             string message, string social, HttpContext context);
    }
}
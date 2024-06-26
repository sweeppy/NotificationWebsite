using Hangfire;
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.Models;
using NotificationWebsite.Utility;
using NotificationWebsite.Utility.Helpers.NotificationActions;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/notificationActions")]
    public class NotificationActionsAPIController : ControllerBase
    {
        private readonly INotificationActions _notiActions;

        public NotificationActionsAPIController(INotificationActions notiActions)
        {
            _notiActions = notiActions;
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteSelectedNotifcations([FromBody] int[] notificationsId)
        {
            if (notificationsId == null) return BadRequest("Empty array.");
            try
            {
                foreach (var notificationId in notificationsId)
                {
                    Notification notification = await _notiActions.GetNotificationByIdAsync(notificationId);
                    if (notification.Status == NotificationStatuses.Planned)
                    {
                        BackgroundJob.Delete(notification.JobId);
                    }
                    await _notiActions.DeleteNotifcationAsync(notification);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("All selected notifications were deleted.");
        }
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelSelectedNotifications([FromBody] int[] notifcationsId)
        {
            if (notifcationsId == null) return BadRequest("Empty array.");

            try
            {
                foreach(var notificationId in notifcationsId)
                {
                    Notification notification = await _notiActions.GetNotificationByIdAsync(notificationId);
                    BackgroundJob.Delete(notification.JobId);
                    notification.Status = NotificationStatuses.Canceled;
                    await _notiActions.UpdateUsersDbAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("All selected notifications were canceled.");
        }
    }
}
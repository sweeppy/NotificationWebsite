using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.Models;
using NotificationWebsite.Utility.Helpers.NotificationActions;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/notifications")]
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
                    Notification notification = await _notiActions.GetNotificationAsync(notificationId);
                    await _notiActions.DeleteNotifcationAsync(notification);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Ok");
        }
    }
}
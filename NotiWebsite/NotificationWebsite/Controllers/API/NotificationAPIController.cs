using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.DataAccess.Contracts;
using NotificationWebsite.Utility.Helpers.NotificationActions;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/NotificationAPI")]
    public class NotificationAPIController : ControllerBase
    {
        private readonly INotificationActions _notificationActions;

        public NotificationAPIController(
            INotificationActions notificationActions)
        {
            _notificationActions = notificationActions;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody]CreateNotificationRequest request, [FromServices] IHttpContextAccessor accessor)
         {
            HttpContext context = accessor.HttpContext;

            var result = await _notificationActions.CreateNotification(request, context);
            switch (result.Value)
            {
                case "success":
                    return Ok(result.Value);
                case "error":
                    return BadRequest(result.Value);
                default:
                    return StatusCode(500, "Unknown result");
            }
         }
        
    }
}
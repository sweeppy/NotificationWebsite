using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.Utility.Configuration.Vkontakte;
using VkNet.Abstractions;
using VkNet.Model;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/vk")]
    public class VkontakteAPIController : ControllerBase
    {

        [HttpPost("vkSendMessage")]
        public async Task<IActionResult> SendVkMessage()
        {    
            return Ok("The message was sent successfully");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.Models.Models;
using Telegram.Bot;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/telegram")]
    public class TelegramAPIController : ControllerBase
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramAPIController(ITelegramBotClient _telegramBot)
        {
            _telegramBotClient = _telegramBot;
        }
        [HttpPost("telegramSendMessage")]
        public async Task<IActionResult> SendTelegramMessage([FromBody]TelegramMessageRequest request)
        {
            try
            {
                var chat = await _telegramBotClient.GetChatAsync(request.Username);

                await _telegramBotClient.SendTextMessageAsync(chat.Id, "Message");
                
                return Ok("Message was sent successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

    }
}
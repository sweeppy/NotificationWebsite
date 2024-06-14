using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    public class TelegramAPIController : ControllerBase
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramAPIController(ITelegramBotClient _telegramBot)
        {
            _telegramBotClient = _telegramBot;
        }
        [HttpPost("telegramSendMessage")]
        public async Task<IActionResult> SendTelegramMessage()
        {
            return null;
        }

    }
}
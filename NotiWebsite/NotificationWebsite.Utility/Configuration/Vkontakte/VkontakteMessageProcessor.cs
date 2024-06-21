
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VkNet.Abstractions;
using VkNet.Model;

namespace NotificationWebsite.Utility.Configuration.Vkontakte
{
    public class VkontakteMessageProcessor : BackgroundService
    {
        private readonly IVkApi _vkApi;

        private readonly ILogger<VkontakteMessageProcessor> _logger;

        public VkontakteMessageProcessor(IVkApi vkApi, ILogger<VkontakteMessageProcessor> logger)
        {
            _vkApi = vkApi;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var longPoll = _vkApi.Messages.GetLongPollServer();

                var updates = await _vkApi.Messages.GetLongPollHistoryAsync(new MessagesGetLongPollHistoryParams
                {
                    Ts = longPoll.Ts,
                    Pts = longPoll.Pts
                });

                foreach (var message in updates.Messages)
                {
                    _logger.LogInformation(message.Text);
                    _logger.LogInformation("new message");
                    await HandleMessageAsync(message);
                }
            }
        }

        private async Task HandleMessageAsync(Message message)
        {
            await _vkApi.Messages.SendAsync(new MessagesSendParams
            {
                UserId = message.FromId,
                Message = "Test message",
                RandomId = new DateTime().Millisecond
            });
        }
    }

}
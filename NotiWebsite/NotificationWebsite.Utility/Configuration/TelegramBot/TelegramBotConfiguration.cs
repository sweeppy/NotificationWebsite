
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using NotificationWebsite.DataAccess.Data;
    using Telegram.Bot;
    using Telegram.Bot.Types;

    namespace NotificationWebsite.Utility.Configuration.TelegramBot
    {
        public class TelegramBotConfiguration : ITelegramBotConfiguration
        {
            private readonly ITelegramBotClient _telegramBotClient;

            private readonly IConfiguration _configuration;

            public TelegramBotConfiguration(ITelegramBotClient telegramBotClient, IConfiguration configuration)
            {
                _telegramBotClient = telegramBotClient;
                _configuration = configuration;
            }

            public void Configure()
            {
                _telegramBotClient.StartReceiving(Update, Error);
            }
            private async Task Update(ITelegramBotClient client, Update update,
            CancellationToken token)
            {
                var message = update.Message;

                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(_configuration.GetConnectionString("UsersConnection"))
                    .Options;

                var dbContext = new ApplicationDbContext(options);

                IUserRepository _userRepository = new UserRepository(dbContext);

                Models.User user = await _userRepository.GetByEmail(message.Text);

                if (user == null)
                {
                    await _telegramBotClient.SendTextMessageAsync(message.Chat, "Wrong email.");
                }
                else
                {
                    user.ChatIdIdentifier = message.Chat.Id;
                    await _userRepository.Update();
                    await client.SendTextMessageAsync(message.Chat, "Your chat id was successfully updated, now you can get notificaiton in telegram! Our website: http://localhost:5019/main/home");
                }
            }
            private async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
            {
                Console.WriteLine(exception.Message);
                throw exception;
            }
        }
    }
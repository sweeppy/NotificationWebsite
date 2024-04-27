using Microsoft.AspNetCore.Mvc;
using NotificationWebsite.DataAccess.Data;
using NotificationWebsite.Models;

namespace NotificationWebsite.Controllers
{
    public class Main : Controller
    {
        private readonly ILogger<Main> _logger;
        private readonly IUserRepository _user_rep;
        public Main(ILogger<Main> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _user_rep = userRepository;
        }

        public async Task<IActionResult> Home()
        {
            /*for test*/
            User user = await _user_rep.GetById(1);
            user.Notifications =
            [
                new Notification
                {
                    Header = "Some header Hello World",
                    SocialNetwork = "Gmail",
                    Message = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Tempora laudantium assumenda nisi similique officia cum rerum. Totam consequatur ut dolorum commodi aliquid hic id nihil accusamus, autem ratione. Omnis, quibusdam.",
                    Status = "Planned",
                    Date = DateTime.Now
                },
                new Notification
                {
                    Header = "Some header Hello World",
                    SocialNetwork = "Telegram",
                    Message = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Tempora laudantium assumenda nisi similique officia cum rerum. Totam consequatur ut dolorum commodi aliquid hic id nihil accusamus, autem ratione. Omnis, quibusdam.",
                    Status = "Sent",
                    Date = DateTime.Now
                },
                new Notification
                {
                    Header = "Some header Hello World",
                    SocialNetwork = "Vkontakte",
                    Message = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Tempora laudantium assumenda nisi similique officia cum rerum. Totam consequatur ut dolorum commodi aliquid hic id nihil accusamus, autem ratione. Omnis, quibusdam.",
                    Status = "Canceled",
                    Date = DateTime.Now
                },
                new Notification
                {
                    Header = "Some header Hello World",
                    SocialNetwork = "Vkontakte",
                    Message = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Tempora laudantium assumenda nisi similique officia cum rerum. Totam consequatur ut dolorum commodi aliquid hic id nihil accusamus, autem ratione. Omnis, quibusdam.",
                    Status = "Canceled",
                    Date = DateTime.Now
                },
                new Notification
                {
                    Header = "Some header Hello World",
                    SocialNetwork = "Vkontakte",
                    Message = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Tempora laudantium assumenda nisi similique officia cum rerum. Totam consequatur ut dolorum commodi aliquid hic id nihil accusamus, autem ratione. Omnis, quibusdam.",
                    Status = "sent",
                    Date = DateTime.Now
                },
            ];
            return View(user);
        }

    }
}
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotificationWebsite.DataAccess.Contracts;
using NotificationWebsite.Models;
using NotificationWebsite.Utility;
using NotificationWebsite.Utility.Helpers.NotificationActions;
using NotificationWebsite.Utility.Jwt;
using NotificationWebsite.Utility.Oauth.OauthHelpers;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/gmail")]
    public class GmailAPIController : ControllerBase
    {

        private readonly INotificationActions _notiActions;
        private readonly IJwtService _jwtService;
        private readonly IClientService _service;

        public GmailAPIController(INotificationActions notiActions, IJwtService jwtService, IClientService clientService)
        {
            _notiActions = notiActions;
            _jwtService = jwtService;
            _service = clientService;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody]CreateNotificationRequest request,
        [FromServices] IHttpContextAccessor accessor)
        {   
            if (request == null)
            {
                return BadRequest("Notification is null");
            }
            var context = accessor.HttpContext;
            User authenticatedUser = await _jwtService.GetUserByTokenAsync(context.Request.Cookies["L_Cookie"]);//get user from jwt token
            Notification notification = _notiActions.MakeNotificationFromRequest(request, authenticatedUser);
            
                try
                {
                    string userEmail = ((GmailService)_service).Users.GetProfile("me").Execute().EmailAddress;

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("", userEmail));
                    message.To.Add(new MailboxAddress("", userEmail));
                    message.Subject = notification.Header;
                    message.Body = new TextPart("plain")
                    {
                        Text = notification.Message
                    };
                    message.Date = notification.Date;

                    using (var stream = new System.IO.MemoryStream())
                    {
                        message.WriteTo(stream);

                        var newMsg = new Message
                        {
                            Raw = SD.Base64UrlEncode(stream.ToArray())
                        };
                    
                        await _notiActions.AddNotificationToDBAsync(notification, authenticatedUser);

                        var delay = notification.Date - DateTime.Now;

                        Task.Run(async () =>
                        {
                            await Task.Delay(delay);

                            await ((GmailService)_service).Users.Messages.Send(newMsg, "me").ExecuteAsync(); // Send message by the end of delay
                        });
                        
                        BackgroundJob.Schedule(() => 
                        _notiActions.UpdateNotificationStatusAsync(notification, authenticatedUser), delay); // Change notification status
                    }                         
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return BadRequest($"{msg}");
                }
            return Ok("Email was sent successfuly");
        }
    }   
    
}
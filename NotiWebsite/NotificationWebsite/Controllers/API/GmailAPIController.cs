using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotificationWebsite.Models.Contracts;
using NotificationWebsite.Models;
using NotificationWebsite.Utility;
using NotificationWebsite.Utility.Helpers.NotificationActions;
using NotificationWebsite.Utility.Jwt;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/gmail")]
    public class GmailAPIController : ControllerBase
    {

        private readonly INotificationActions _notiActions;
        private readonly IJwtService _jwtService;
        private readonly GmailService _service;

        public GmailAPIController(INotificationActions notiActions,
         IJwtService jwtService, GmailService clientService)
        {
            _notiActions = notiActions;
            _jwtService = jwtService;
            _service = clientService;
        }

        [HttpPost("gmailSendMessage")]
        public async Task<IActionResult> GmailSendMessage([FromBody]CreateNotificationRequest request,
        [FromServices] IHttpContextAccessor accessor)
        {   
            if (request == null)
            {
                return BadRequest("Notification is null");
            }

            //get user from jwt token
            User authenticatedUser = await _jwtService.GetUserByTokenAsync(accessor.HttpContext.Request.Cookies["L_Cookie"]);

            //make notification model from requset body
            Notification notification = _notiActions.MakeNotificationFromRequest(request, authenticatedUser);

            if (authenticatedUser == null || notification == null)
            {
                return BadRequest("User information not found");
            }

            try
                {
                    string userEmail = _service.Users.GetProfile("me").Execute().EmailAddress;

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

                        // Send message on his email

                        // Change notification status
                        notification.JobId =  BackgroundJob.Schedule(() => 
                        _notiActions.SendAndUpdateNotificationGmail(
                            notification, authenticatedUser, newMsg),delay);
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
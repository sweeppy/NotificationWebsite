using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotificationWebsite.Models;
using NotificationWebsite.Utility;
using NotificationWebsite.Utility.Oauth.Load;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/gmail")]
    public class GmailAPIController : ControllerBase
    {
        [HttpPost("sendMessage")]
        public IActionResult SendMessage(IConfiguration configuration, [FromBody]Notification notification)
        {   
            if (notification == null || notification.Date < DateTime.Now)
            {
                return BadRequest("Invalid notification");
            }

            OAuthClientInfo clientInfo =  OAuthClientInfo.Load(configuration);//get client id and secret from appsettings
            
            ClientSecrets secrets = new ClientSecrets()//initialize client_id and secret
            {
                ClientId = clientInfo.ClientId,
                ClientSecret = clientInfo.ClientSecret
            };

            string [] scopes = new string[] {Google.Apis.Gmail.v1.GmailService.Scope.GmailSend,//for send gmail
                                            Google.Apis.Gmail.v1.GmailService.Scope.GmailCompose};//for get user email address

            UserCredential credential = GoogleWebAuthorizationBroker.
                AuthorizeAsync(secrets, scopes, "user", CancellationToken.None).Result;
            
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "SendMessage",
            });
                try
                {
                    string userEmail = service.Users.GetProfile("me").Execute().EmailAddress;
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
                        System.Threading.Timer timer = null;
                        var delay = notification.Date - DateTime.Now;//culculating delay for disposing thread

                        timer = new System.Threading.Timer(async (state) =>
                        {
                            service.Users.Messages.Send(newMsg, "me").Execute();
                            timer.Dispose();
                        }, null, delay, TimeSpan.Zero);
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
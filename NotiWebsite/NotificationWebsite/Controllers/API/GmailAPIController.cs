using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotificationWebsite.Utility;
using NotificationWebsite.Utility.Oauth.Load;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/gmail")]
    public class GmailAPIController : ControllerBase
    {
        [HttpPost("sendMessage")]
        public IActionResult SendMessage(IConfiguration configuration)
        {
            OAuthClientInfo clientInfo =  OAuthClientInfo.Load(configuration);
            
            ClientSecrets secrets = new ClientSecrets()
            {
                ClientId = clientInfo.ClientId,
                ClientSecret = clientInfo.ClientSecret
            };

            string [] scopes = new string[] {Google.Apis.Gmail.v1.GmailService.Scope.GmailSend,
                                            Google.Apis.Gmail.v1.GmailService.Scope.GmailCompose};

            UserCredential credential = GoogleWebAuthorizationBroker.
                AuthorizeAsync(secrets, scopes, "user", CancellationToken.None).Result;
            
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ApplicationName",
            });

                    var userInfoRequest = service.Users.GetProfile("me");
                    try
                    {
                        var userInfo = userInfoRequest.Execute();
                        var userEmail = userInfo.EmailAddress;
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                        return BadRequest("Get email error");
                    }


            var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Test Name", "@gmail.com"));
                message.To.Add(new MailboxAddress("", "lovethemoon808@gmail.com"));
                message.Subject = "Test Email Using Gmail API";

                message.Body = new TextPart("plain")
                {
                    Text = "This is a test email sent using the Gmail API."
                };

                using (var stream = new System.IO.MemoryStream())
                {
                    message.WriteTo(stream);

                    var newMsg = new Message
                    {
                        Raw = SD.Base64UrlEncode(stream.ToArray())
                    };
                    service.Users.Messages.Send(newMsg, "me").Execute();
                }
            

            return Ok();
        }
    }   
    
}
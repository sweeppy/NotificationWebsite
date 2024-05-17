using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NotificationWebsite.DataAccess.Contracts;
using NotificationWebsite.Models;
using NotificationWebsite.Utility;
using NotificationWebsite.Utility.Helpers.NotificationActions;
using NotificationWebsite.Utility.Jwt;
using NotificationWebsite.Utility.Oauth.Load;
using NotificationWebsite.Utility.Oauth.OauthHelpers;

namespace NotificationWebsite.Controllers.API
{
    [ApiController]
    [Route("api/gmail")]
    public class GmailAPIController : ControllerBase
    {

        private readonly INotificationActions _notiActions;
        private readonly IJwtService _jwtService;
        private readonly ISecrets _secrets;
        private readonly IConfiguration _configuration;

        public GmailAPIController(INotificationActions notiActions, IJwtService jwtService, ISecrets secrets, IConfiguration configuration)
        {
            _notiActions = notiActions;
            _jwtService = jwtService;
            _secrets = secrets;
            _configuration = configuration;
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
            

            OAuthClientInfo clientInfo =  OAuthClientInfo.Load(_configuration);//get client id and secret from appsettings
            ClientSecrets secrets = _secrets.GetSecrets(clientInfo);

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
                            service.Users.Messages.Send(newMsg, "me").Execute();//send message by the end of delay
                            await _notiActions.UpdateNotificationStatusAsync(notification, authenticatedUser);//changing notification status 
                            timer.Dispose();
                        }, null, delay, TimeSpan.Zero);
                    }

                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return BadRequest($"{msg}");
                }
            try
            {
                await _notiActions.AddNotificationToDBAsync(notification, authenticatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"An exeption while saving changes in DB{ex.Message}");
            }
            return Ok("Email was sent successfuly");
        }
    }   
    
}
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationWebsite.Utility.Oauth.Load;

namespace NotificationWebsite.Utility.Oauth.Configuration
{
    public class Oauth2Configuration()
    {
        public void ConfigureOauth2(IServiceCollection services, IConfiguration configuration)
        {
            OAuthClientInfo clientInfo =  OAuthClientInfo.Load(configuration);

            var credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientInfo.ClientId,
                    ClientSecret = clientInfo.ClientSecret,
                },
                new[] { GmailService.Scope.GmailSend, GmailService.Scope.GmailCompose },
                "me",
                CancellationToken.None,
                new FileDataStore("Gmail.Credentials")
            ).Result;

            var initializer = new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = "SendNotification"
            };

            var gmailService = new GmailService(initializer);

            services.AddSingleton(credentials);

            services.AddSingleton(gmailService);

        }
    }
}
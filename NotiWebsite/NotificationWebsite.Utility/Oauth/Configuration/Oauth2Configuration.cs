
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            services
                .AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;

                    options.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;

                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(op =>
                {
                })
                .AddGoogleOpenIdConnect(o =>
                {
                    o.ClientId = clientInfo.ClientId;
                    o.ClientSecret = clientInfo.ClientSecret;
                });
        }
    }
}
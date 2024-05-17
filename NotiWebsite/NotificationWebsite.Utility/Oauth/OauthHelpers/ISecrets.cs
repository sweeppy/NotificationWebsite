using Google.Apis.Auth.OAuth2;
using NotificationWebsite.Utility.Oauth.Load;

namespace NotificationWebsite.Utility.Oauth.OauthHelpers
{
    public interface ISecrets
    {
        public ClientSecrets GetSecrets(OAuthClientInfo clientInfo);
    }
}
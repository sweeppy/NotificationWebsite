
using Google.Apis.Auth.OAuth2;
using NotificationWebsite.Utility.Oauth.Load;

namespace NotificationWebsite.Utility.Oauth.OauthHelpers
{
    public class Secrets: ISecrets
    {
        public ClientSecrets GetSecrets(OAuthClientInfo clientInfo)
        {
            ClientSecrets secrets = new ClientSecrets
            {
                ClientId = clientInfo.ClientId,
                ClientSecret = clientInfo.ClientSecret
            };
            return secrets;
        }
    }
}
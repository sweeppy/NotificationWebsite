
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace NotificationWebsite.Utility.Oauth.Load
{
    public class OAuthClientInfo
    {

        public static OAuthClientInfo Load(IConfiguration configuration)
        {
            string SecretFilePath = configuration.GetSection("SECRET_FILE_NAME").Value;

            if (string.IsNullOrEmpty(SecretFilePath))
            {
                throw new InvalidOperationException($"Need to set the section <SECRET_FILE_NAME> in appsetting.json");
            }

            var secrets = JObject.Parse(File.ReadAllText(SecretFilePath))["web"];
            if (secrets is null)
            {
                throw new InvalidOperationException($"secrets is null");
            }

            var projectId = secrets["project_id"].Value<string>();
            var clientId = secrets["client_id"].Value<string>();
            var clientSecret = secrets["client_secret"].Value<string>();

            return new OAuthClientInfo(projectId, clientId, clientSecret);
        }

        private OAuthClientInfo(string projectId, string clientId, string clientSecret)
        {
            ProjectId = projectId;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ProjectId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
    }
}
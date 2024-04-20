

namespace NotificationWebsite.Utility.Configuration.Jwt
{
    public class JwtConfig
    {
        public string SecretKey { get; set; } = string.Empty;

        public int ExpireHours { get; set; }
    }
}
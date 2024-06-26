
namespace NotificationWebsite.Utility
{
    public static class SD//static details
    {
        public static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }

    public static class NotificationStatuses
{
    public const string Planned = "planned";
    public const string Canceled = "canceled";
    public const string Sent = "sent";
}
}

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
    public const string Planned = "Planned";
    public const string Canceled = "Canceled";
    public const string Sent = "Sent";
}
}
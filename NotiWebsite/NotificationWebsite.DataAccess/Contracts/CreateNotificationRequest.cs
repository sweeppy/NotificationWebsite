
namespace NotificationWebsite.DataAccess.Contracts
{
    public record CreateNotificationRequest(DateTime dateTimeParam, string header, string message,
             string social);
}
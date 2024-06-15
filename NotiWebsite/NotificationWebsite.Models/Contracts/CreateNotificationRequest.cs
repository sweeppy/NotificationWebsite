
namespace NotificationWebsite.Models.Contracts
{
    public record CreateNotificationRequest(DateTime dateTimeParam, string header, string message,
             string social);
}
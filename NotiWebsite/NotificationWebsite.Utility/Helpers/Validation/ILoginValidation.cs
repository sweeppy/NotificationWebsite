using NotificationWebsite.Models;

namespace NotificationWebsite.Utility.Helpers.Validation
{
    public interface ILoginValidation
    {
        public bool IsLoginValid(User user);
    }
}
using NotificationWebsite.Models;

namespace NotificationWebsite.Utility.Authentication
{
    public interface ILoginValidation
    {
        public bool IsLoginValid(User user);
    }
}
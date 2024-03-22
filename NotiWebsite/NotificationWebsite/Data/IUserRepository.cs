using NotificationWebsite.Models;

namespace NotificationWebsite.Data
{
    public interface IUserRepository
    {
        User Create(User user);
    }
}
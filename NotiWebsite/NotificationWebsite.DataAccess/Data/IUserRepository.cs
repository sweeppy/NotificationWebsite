using NotificationWebsite.Models;

namespace NotificationWebsite.DataAccess.Data
{
    public interface IUserRepository
    {
        User Create(User user);
    }
}
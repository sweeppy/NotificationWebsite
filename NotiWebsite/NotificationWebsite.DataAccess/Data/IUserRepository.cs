using NotificationWebsite.Models;

namespace NotificationWebsite.DataAccess.Data
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<User> GetByEmail(string? email);
        Task<User> GetById(int id);
    }
}
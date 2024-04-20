using Microsoft.EntityFrameworkCore;
using NotificationWebsite.Models;

namespace NotificationWebsite.DataAccess.Data
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<User> Create(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetByEmail(string? email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetById(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
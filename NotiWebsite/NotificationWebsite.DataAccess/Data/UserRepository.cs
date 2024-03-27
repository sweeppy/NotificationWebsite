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
        public User Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }
        public User GetByEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email) ?? throw new Exception("User in not found");
        }

        public User GetById(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id) ?? throw new Exception("User is not found");
        }
    }
}
using Microsoft.EntityFrameworkCore;
using NotificationWebsite.Models;

namespace NotificationWebsite.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
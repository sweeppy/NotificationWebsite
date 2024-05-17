using System.Security.Claims;
using NotificationWebsite.Models;

namespace NotificationWebsite.Utility.Jwt
{
    public interface IJwtService
    {
        public string Generate(int id);

        public Task<User> GetUserByTokenAsync(string token);

        public ClaimsPrincipal GetClaimsPrincipalFromToken(string token);
    }
}
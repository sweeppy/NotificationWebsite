using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using NotificationWebsite.Utility.Configuration.Jwt;
using System.Security.Claims;
using NotificationWebsite.Models;

using NotificationWebsite.DataAccess.Data;

namespace NotificationWebsite.Utility.Helpers.Jwt
{
    
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _jwtConfig;

        private readonly IUserRepository _repository;

        public JwtService(IOptionsMonitor<JwtConfig> optionsMonitor,
         IUserRepository repository)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _repository = repository;
        }
        public string Generate(int id)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("UserId", id.ToString()),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(id.ToString(), null, claims, null, DateTime.Now.AddHours(_jwtConfig.ExpireHours));//token will live 1 hour
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);
            var parametrs = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, parametrs, out var validatedToken);
            return principal;
        }

        public async Task<User> GetUserByToken(string token)
        {
            var principal = GetClaimsPrincipalFromToken(token);

            var userIdClaim = principal?.FindFirst("UserId");

            if (userIdClaim == null && int.TryParse(userIdClaim.Value, out int userId))
            {
                User authenticatedUser = await _repository.GetById(userId);
                return authenticatedUser;
            }
            return null;
        }
    }
}
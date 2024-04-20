using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using NotificationWebsite.Utility.Configuration.Jwt;
using System.Security.Claims;

namespace NotificationWebsite.Utility.Helpers.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _jwtConfig;

        public JwtService(IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
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
    }
}
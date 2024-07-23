using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class JWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            string jwtKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("No secret key in application settings.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("No issuer in application settings."),
               audience: _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("No audience in application settings."),
               claims: claims,
               expires: DateTime.UtcNow.AddHours(1),
               signingCredentials: credentials
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

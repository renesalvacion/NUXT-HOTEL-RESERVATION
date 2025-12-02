using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUD.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(string userId, string email, string role = "")
        {
            // Load JWT settings from configuration
            var jwtSettings = _config.GetSection("Jwt");
            var keyString = jwtSettings["Key"] ?? throw new ArgumentNullException("JWT Key not configured");
            var issuer = jwtSettings["Issuer"] ?? throw new ArgumentNullException("JWT Issuer not configured");
            var audience = jwtSettings["Audience"] ?? throw new ArgumentNullException("JWT Audience not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

            // Only add role claim if it is not empty
            if (!string.IsNullOrWhiteSpace(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Token expiration in minutes (from config or default to 30)
            int tokenValidityMins = int.TryParse(jwtSettings["TokenValidityMins"], out var mins) ? mins : 30;
            var expires = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            // Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

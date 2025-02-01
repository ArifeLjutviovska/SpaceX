namespace SpacexServer.Api.Common.Security
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using SpacexServer.Storage.Users.Entities;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class JwtService(IConfiguration configuration)
    {
        private readonly string _secret = configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JWT Secret is missing");
        private readonly string _issuer = configuration["JwtSettings:Issuer"]!;
        private readonly string _audience = configuration["JwtSettings:Audience"]!;
        private readonly int _expiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"]!);

        public string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: _issuer,
                                             audience: _audience,
                                             claims: claims,
                                             expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
                                             signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("=", "")
                .Replace("+", "")
                .Replace("/", "");
        }
    }
}
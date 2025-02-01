namespace SpacexServer.Api.Common.Security
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.IdentityModel.Tokens;
    using SpacexServer.Storage.Users.Entities;

    public class JwtService
    {

        private readonly string _jwtSecret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpiryMinutes;
        private readonly int _refreshTokenExpiryDays;

        public JwtService(IConfiguration configuration)
        {
            _jwtSecret = configuration["JwtSettings:Secret"] ?? Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? throw new ArgumentNullException("JWT secret is missing.");

            _issuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JWT issuer is missing.");
            _audience = configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JWT audience is missing.");
            _accessTokenExpiryMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpiryMinutes"] ?? "15");
            _refreshTokenExpiryDays = int.Parse(configuration["JwtSettings:RefreshTokenExpiryDays"] ?? "7");
        }

        /// <summary>
        /// Generates a JWT access token for a given user.
        /// </summary>
        public string GenerateAccessToken(User user)
        {
            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(_jwtSecret);
            }
            catch (FormatException)
            {
                keyBytes = Encoding.UTF8.GetBytes(_jwtSecret);
            }

            if (keyBytes.Length < 16)
            {
                throw new ArgumentException("JWT secret key must be at least 16 bytes (128 bits).");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                ]),
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256),
                Issuer = _issuer,
                Audience = _audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates a cryptographically secure refresh token.
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

    }
}

namespace SpacexServer.Api.Common.Security
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using SpacexServer.Storage.Users.Entities;

    /// <summary>
    /// Service for generating and managing JWT authentication tokens.
    /// This includes creating access tokens for authentication and refresh tokens for session renewal.
    /// Uses HMAC SHA-256 encryption for secure token signing.
    /// </summary>
    public class JwtService(IConfiguration configuration)
    {

        private readonly string _jwtSecret = configuration["JwtSettings:Secret"] ?? Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? throw new ArgumentNullException("JWT secret is missing.");
        private readonly string _issuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JWT issuer is missing.");
        private readonly string _audience = configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JWT audience is missing.");
        private readonly int _accessTokenExpiryMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpiryMinutes"] ?? "15");
        private readonly int _refreshTokenExpiryDays = int.Parse(configuration["JwtSettings:RefreshTokenExpiryDays"] ?? "7");

        /// <summary>
        /// Generates a JWT access token for the specified user.
        /// The token includes user claims (ID, email, first & last name) and is signed with HMAC SHA-256.
        /// The expiration time is configured via app settings.
        /// </summary>
        /// <param name="user">User entity containing ID, email, first & last name.</param>
        /// <returns>A signed JWT access token as a string.</returns>

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
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName)
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
        /// Generates a secure, cryptographically random refresh token.
        /// Used to maintain authentication without requiring the user to log in again.
        /// </summary>
        /// <returns>A base64-encoded refresh token string.</returns>
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
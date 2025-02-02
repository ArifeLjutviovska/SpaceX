namespace SpacexServer.Storage.RefreshTokens.Entities
{
    using SpacexServer.Storage.Users.Entities;

    /// <summary>
    /// Represents a refresh token used for authentication.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Gets the unique identifier for the refresh token.
        /// </summary>
        public int Id { get; protected internal set; }

        /// <summary>
        /// Gets the foreign key associated with the user.
        /// </summary>
        public int UserFk { get; protected internal set; }

        /// <summary>
        /// Gets the refresh token string.
        /// </summary>
        public string Token { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets the timestamp indicating when the token expires.
        /// </summary>
        public DateTime ExpiresAt { get; protected internal set; }

        /// <summary>
        /// Gets the timestamp when the token was created.
        /// </summary>
        public DateTime CreatedAt { get; protected internal set; }

        /// <summary>
        /// Gets the timestamp when the token was revoked (if applicable).
        /// </summary>
        public DateTime? RevokedAt { get; protected internal set; }

        /// <summary>
        /// Gets the user associated with this refresh token.
        /// </summary>
        public virtual User User { get; protected internal set; } = null!;

        /// <summary>
        /// Creates a new refresh token instance.
        /// </summary>
        /// <param name="userFk">The foreign key of the user.</param>
        /// <param name="token">The generated token string.</param>
        /// <param name="expiresAt">The expiration date of the token.</param>
        /// <returns>A new <see cref="RefreshToken"/> instance.</returns>
        public static RefreshToken Create(int userFk, string token, DateTime expiresAt)
        {
            return new RefreshToken
            {
                UserFk = userFk,
                Token = token,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Marks the refresh token as revoked.
        /// </summary>
        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }
    }
}
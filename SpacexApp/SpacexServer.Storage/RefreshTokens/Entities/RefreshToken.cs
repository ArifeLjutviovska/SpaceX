namespace SpacexServer.Storage.RefreshTokens.Entities
{
    using SpacexServer.Storage.Users.Entities;

    public class RefreshToken
    {
        public int Id { get; protected internal set; }

        public int UserFk { get; protected internal set; }

        public string Token { get; protected internal set; } = string.Empty;

        public DateTime ExpiresAt { get; protected internal set; }

        public DateTime CreatedAt { get; protected internal set; }

        public DateTime? RevokedAt { get; protected internal set; }

        public virtual User User { get; protected internal set; } = null!;

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
        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }

    }
}
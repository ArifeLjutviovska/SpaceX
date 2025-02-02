namespace SpacexServer.Storage.RefreshTokens.Repositories
{
    using SpacexServer.Storage.RefreshTokens.Entities;

    /// <summary>
    /// Defines methods for refresh token data access operations.
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Retrieves a refresh token entity by its token string.
        /// </summary>
        /// <param name="token">The refresh token string.</param>
        /// <returns>The refresh token entity if found; otherwise, null.</returns>
        Task<RefreshToken?> GetByTokenAsync(string token);

        /// <summary>
        /// Inserts a new refresh token into the database.
        /// </summary>
        /// <param name="refreshToken">The refresh token entity to insert.</param>
        Task InsertAsync(RefreshToken refreshToken);

        /// <summary>
        /// Updates an existing refresh token in the database.
        /// </summary>
        /// <param name="refreshToken">The refresh token entity with updated data.</param>
        Task UpdateAsync(RefreshToken refreshToken);
    }
}
namespace SpacexServer.Storage.RefreshTokens.Repositories
{
    using SpacexServer.Storage.RefreshTokens.Entities;

    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task InsertAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
    }
}
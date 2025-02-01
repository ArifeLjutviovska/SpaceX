namespace SpacexServer.Storage.RefreshTokens.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SpacexServer.Storage.Common.Context;
    using SpacexServer.Storage.RefreshTokens.Entities;
    using System.Threading.Tasks;

    public class RefreshTokenRepository(SpacexDbContext dbContext) : IRefreshTokenRepository
    {
        private readonly SpacexDbContext _dbContext = dbContext;

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task InsertAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Update(refreshToken);
            await _dbContext.SaveChangesAsync();
        }
    }
}
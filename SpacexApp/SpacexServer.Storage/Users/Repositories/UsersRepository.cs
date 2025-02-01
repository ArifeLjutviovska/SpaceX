namespace SpacexServer.Storage.Users.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using SpacexServer.Storage.Common.Context;
    using SpacexServer.Storage.Users.Entities;

    public class UsersRepository(ISpacexDbContext dbContext) : IUsersRepository
    {
        private readonly ISpacexDbContext _dbContext = dbContext;

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id) 
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task InsertAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
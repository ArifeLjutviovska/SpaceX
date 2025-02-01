namespace SpacexServer.Storage.Users.Repositories
{
    using SpacexServer.Storage.Users.Entities;

    public interface IUsersRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByIdAsync(int id);

        Task InsertAsync(User user);
    }
}
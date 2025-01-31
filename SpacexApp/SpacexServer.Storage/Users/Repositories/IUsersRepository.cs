namespace SpacexServer.Storage.Users.Repositories
{
    using SpacexServer.Storage.Users.Entities;

    public interface IUsersRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task InsertAsync(User user);
    }
}
namespace SpacexServer.Contracts.User.Repositories
{
    using SpacexServer.Entities.User.Domain;

    public interface IUserRepository
    {
        int Insert(User user);
    }
}
namespace SpacexServer.Storage.User.Repositories
{
    using SpacexServer.Contracts.User.Repositories;
    using SpacexServer.Storage.Common.Context;
    using SpacexServer.Storage.Common.Repositories;
    using SpacexServer.Storage.User.Entities;
    using SpacexServer.Storage.User.Factories;

    public class UserRepository(ISpacexDbContext spacexDbContext) : Repository<User>(spacexDbContext), IUserRepository
    {
        public int Insert(SpacexServer.Entities.User.Domain.User user)
        {
            User dbUser = user.ToUserDb();
            
            Insert(dbUser);

            return dbUser.Id;
        }
    }
}
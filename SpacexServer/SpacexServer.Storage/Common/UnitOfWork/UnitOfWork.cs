namespace SpacexServer.Storage.Common.UnitOfWork
{
    using SpacexServer.Contracts.Common.Repositories;
    using SpacexServer.Storage.Common.Context;
    using System.Threading.Tasks;

    public class UnitOfWork(ISpacexDbContext spacexDbContext) : IUnitOfWork
    {
        private readonly ISpacexDbContext _spacexDbContext = spacexDbContext;

        public async Task SaveAsync()
        {
            await _spacexDbContext.SaveChangesAsync();
        }
    }
}
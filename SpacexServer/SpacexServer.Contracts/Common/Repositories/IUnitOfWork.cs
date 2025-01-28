namespace SpacexServer.Contracts.Common.Repositories
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}
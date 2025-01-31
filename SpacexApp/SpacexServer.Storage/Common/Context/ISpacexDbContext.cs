namespace SpacexServer.Storage.Common.Context
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using SpacexServer.Storage.Users.Entities;

    public interface ISpacexDbContext : IDisposable
    {
        DbSet<User> Users { get; set; }

        ChangeTracker ChangeTracker { get; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
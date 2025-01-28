namespace SpacexServer.Storage.Common.Context
{
    using Microsoft.EntityFrameworkCore;
    using SpacexServer.Storage.Common.Configuration;

    public class SpacexDbContext(DbContextOptions<SpacexDbContext> options) : DbContext(options), ISpacexDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMapping());
        }
    }
}
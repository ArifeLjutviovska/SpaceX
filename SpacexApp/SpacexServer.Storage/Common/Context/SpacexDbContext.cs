namespace SpacexServer.Storage.Common.Context
{
    using Microsoft.EntityFrameworkCore;
    using SpacexServer.Storage.Common.Configuration;
    using SpacexServer.Storage.Users.Entities;

    public class SpacexDbContext(DbContextOptions<SpacexDbContext> options) : DbContext(options), ISpacexDbContext
    {

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UsersMapping());
        }
    }
}
namespace SpacexServer.Storage.Common.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SpacexServer.Storage.User.Entities;

    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "Spacex");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(150);
            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("datetime2(0)").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("datetime2(0)").HasDefaultValue(null);
        }
    }
}

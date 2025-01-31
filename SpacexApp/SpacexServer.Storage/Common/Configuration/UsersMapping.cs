namespace SpacexServer.Storage.Common.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SpacexServer.Storage.Users.Entities;

    public class UsersMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "Spacex");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedOn).HasColumnName("CreatedOn").HasColumnType("datetime2(0)").IsRequired();
            builder.Property(x => x.DeletedOn).HasColumnName("DeletedOn").HasColumnType("datetime2(0)").HasDefaultValue(null);

            builder.Property(x => x.Email).HasColumnName("Email").HasColumnType("NVARCHAR").HasMaxLength(75).IsRequired();
            builder.Property(x => x.FirstName).HasColumnName("FirstName").HasColumnType("NVARCHAR").HasMaxLength(150).IsRequired();
            builder.Property(x => x.LastName).HasColumnName("LastName").HasColumnType("NVARCHAR").HasMaxLength(150).IsRequired();
            builder.Property(x => x.Password).HasColumnName("Password").HasColumnType("NVARCHAR").HasMaxLength(150).IsRequired();

            builder.HasIndex(x => x.Email).HasDatabaseName("IX_Users_Email").IsUnique();
        }
    }
}
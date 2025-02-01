namespace SpacexServer.Storage.Common.Configuration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SpacexServer.Storage.RefreshTokens.Entities;

    public class RefreshTokensMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens", "Spacex");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(x => x.UserFk).HasColumnName("UserId").IsRequired();
            builder.Property(x => x.Token).HasColumnName("Token").HasColumnType("NVARCHAR").HasMaxLength(255).IsRequired();
            builder.Property(x => x.ExpiresAt).HasColumnName("ExpiresAt").HasColumnType("DATETIME2(0)").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("DATETIME2(0)").IsRequired().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.RevokedAt).HasColumnName("RevokedAt").HasColumnType("DATETIME2(0)").HasDefaultValue(null);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserFk).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserFk).HasDatabaseName("IX_RefreshTokens_UserFk");
            builder.HasIndex(x => x.Token).HasDatabaseName("IX_RefreshTokens_Token").IsUnique();
        }
    }
}
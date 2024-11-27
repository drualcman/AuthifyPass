using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthifyPass.API.DataBaseContext.EF.Configurations;
internal class UserSecretEntityConfiguration : IEntityTypeConfiguration<UserSecretEntity>
{
    public void Configure(EntityTypeBuilder<UserSecretEntity> builder)
    {
        builder
            .HasKey(x => new { x.ClientId, x.UserId });
        builder
            .Property(x => x.ClientId)
            .IsRequired()
            .HasColumnType("CHAR(32)");
        builder
            .Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(x => x.ActiveSharedSecret)
            .HasMaxLength(100);
        builder
            .Property(x => x.PreviousSharedSecret)
            .HasMaxLength(100);

        builder
            .HasOne(x => x.Client)
            .WithMany()
            .HasPrincipalKey(x => x.ClientId)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthifyPass.API.DataBaseContext.EF.Configurations;
internal class ClientEntityConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder
            .HasKey(x => x.Id);
        builder
            .HasIndex(x => x.ClientId)
            .IsUnique();
        builder
            .Property(x => x.ClientId)
            .IsRequired()
            .HasColumnType("VARCHAR(32)");
        builder
            .Property(x => x.SharedSecret)
            .IsRequired()
            .HasColumnType("VARCHAR(64)");
        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);
        builder
            .Property(x => x.Name)
            .HasMaxLength(256);
        builder
            .Property(x => x.Password)
            .HasColumnType("VARCHAR(64)");
    }
}

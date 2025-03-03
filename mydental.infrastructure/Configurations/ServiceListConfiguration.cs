using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mydental.domain.Entities;

namespace mydental.infrastructure.Configurations
{
    public class ServiceListConfiguration : IEntityTypeConfiguration<ServiceList>
    {
        public void Configure(EntityTypeBuilder<ServiceList> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ServiceName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("VARCHAR(255)");

            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("VARCHAR(255)");

            builder.Property(s => s.Content)
                .IsRequired()
                .HasColumnType("TEXT"); // ✅ Using TEXT for larger content storage

            builder.Property(s => s.Photo)
                .HasMaxLength(500)
                .HasColumnType("VARCHAR(500)")
                .IsRequired(false);

            // ✅ Add CreatedAt and UpdatedAt properties
            builder.Property(s => s.CreatedAt)
                .IsRequired()
                .HasColumnType("DATETIME2");

            builder.Property(s => s.UpdatedAt)
                .IsRequired()
                .HasColumnType("DATETIME2");
        }
    }
}

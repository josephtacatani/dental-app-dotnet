using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mydental.domain.Entities;

namespace mydental.infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("VARCHAR(255)"); // ✅ Explicitly set to VARCHAR

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasColumnType("VARCHAR(500)"); // ✅ Explicitly set to VARCHAR

            builder.Property(u => u.PasswordSalt) // ✅ Add Salt Column
                .IsRequired()
                .HasColumnType("VARCHAR(500)"); // ✅ Explicitly set to VARCHAR

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("VARCHAR(50)");

            builder.Property("_birthDate")
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(u => u.Photo)
                .HasMaxLength(500)
                .HasColumnType("VARCHAR(500)")
                .IsRequired(false);

            builder.Property(u => u.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("VARCHAR(50)");

            builder.Property(u => u.RefreshToken)
                .HasMaxLength(512)
                .IsUnicode(false) // ✅ Ensures non-Unicode (VARCHAR)
                .HasColumnType("VARCHAR(512)")
                .IsRequired(false);

            builder.Property("_refreshTokenExpiry")
                .IsRequired(false)
                .HasColumnType("DATETIME2");

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("DATETIME2");

            builder.Property(u => u.UpdatedAt)
                .IsRequired()
                .HasColumnType("DATETIME2");
        }
    }
}

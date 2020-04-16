using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.Property(x => x.Id)
                .HasColumnType("text")
                .HasMaxLength(36)
                .IsRequired();

            builder.HasIndex(x => x.FullName).HasName("FullNameIndex");
            builder.HasIndex(x => x.FirstName).HasName("FirstNameIndex");
            builder.HasIndex(x => x.LastName).HasName("LastNameIndex");

            builder.Property(x => x.UserName)
                .HasColumnType("text")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.NormalizedUserName)
                .HasColumnType("text")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnType("text")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.NormalizedEmail)
                .HasColumnType("text")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.FullName)
                .HasColumnType("text")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasColumnType("text")
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .HasColumnType("text")
                .HasMaxLength(100);
        }
    }
}
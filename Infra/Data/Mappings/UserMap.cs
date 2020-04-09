using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers");

            builder.HasIndex(x => x.FullName);
            builder.HasIndex(x => x.FirstName);
            builder.HasIndex(x => x.LastName);

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
using System.Threading.Tasks;
using Domain.Entities;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasIndex(x => x.FullName).IsUnique().HasName("FullNameIndex");
            builder.HasIndex(x => x.FirstName).HasName("FirstNameIndex");
            builder.HasIndex(x => x.LastName).HasName("LastNameIndex");

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
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("AspNetRoles");
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name)
                .HasName("NameIndex");

            builder.HasIndex(x => x.NormalizedName)
                .HasName("NormalizedNameIndex");


            builder.Property(x => x.Id)
                .HasColumnType("text")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnType("text")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.NormalizedName)
                .HasColumnType("text")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
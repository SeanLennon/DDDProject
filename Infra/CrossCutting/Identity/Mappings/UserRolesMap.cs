using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Mappings
{
    public class UserRolesMap : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.ToTable("AspNetUserRoles");
            builder.HasKey(x => new { x.UserId, x.RoleId });

            builder.Property(x => x.UserId)
                .HasColumnType("text")
                .HasMaxLength(36)
                .IsRequired();

            builder.Property(x => x.RoleId)
                .HasColumnType("text")
                .HasMaxLength(36)
                .IsRequired();
        }
    }
}
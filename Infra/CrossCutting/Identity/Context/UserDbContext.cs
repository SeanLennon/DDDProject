using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Identity.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Identity.Context
{
    public class UserDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        protected UserDbContext() { }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public override DbSet<User> Users { get; set; }
        public override DbSet<IdentityRole> Roles { get; set; }
        public override DbSet<IdentityUserRole<string>> UserRoles { get; set; }

        private IDbContextTransaction _transaction;

        public Task BeginTransactionAsync()
        {
            return Task.Run(async () =>
            {
                _transaction = await Database.BeginTransactionAsync();
            });
        }

        public Task CommitAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    await SaveChangesAsync();
                    await _transaction.CommitAsync();
                }
                finally
                {
                    await _transaction.DisposeAsync();
                }
            });
        }

        public Task RollbackAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    await _transaction.RollbackAsync();
                }
                finally
                {
                    await _transaction.DisposeAsync();
                    GC.SuppressFinalize(true);
                }
            });
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new RoleMap());
            builder.ApplyConfiguration(new UserRolesMap());


            var noneRole = new IdentityRole("None") { NormalizedName = "NONE" };
            var userRole = new IdentityRole("User") { NormalizedName = "USER" };
            var adminRole = new IdentityRole("Admin") { NormalizedName = "ADMIN" };
            var managerRole = new IdentityRole("Manager") { NormalizedName = "MANAGER" };

            builder.Entity<IdentityRole>()
                .HasData(noneRole, userRole, adminRole, managerRole);

            string email = "seantpd@gmail.com";
            string username = "dev_master";
            string name = "Sean Ono Lennon Pessoa";
            var roles = new List<string>(){ "Manager" };
            User user = new User(name, username, email, roles);

            builder.Entity<User>()
                .HasData(user);

            var claims = new List<IdentityUserClaim<string>>
            {
                new IdentityUserClaim<string>()
                {
                    Id = 1,
                    UserId = user.Id,
                    ClaimType = "UserName",
                    ClaimValue = user.UserName
                },
                new IdentityUserClaim<string>()
                {
                    Id = 2,
                    UserId = user.Id,
                    ClaimType = "Name",
                    ClaimValue = user.ToString()
                },
                new IdentityUserClaim<string>()
                {
                    Id = 5,
                    UserId = user.Id,
                    ClaimType = "FullName",
                    ClaimValue = user.FullName
                },
                new IdentityUserClaim<string>()
                {
                    Id = 6,
                    UserId = user.Id,
                    ClaimType = "Email",
                    ClaimValue = user.Email
                },
                new IdentityUserClaim<string>()
                {
                    Id = 7,
                    UserId = user.Id,
                    ClaimType = "UserId",
                    ClaimValue = user.Id
                }
            };

            builder.Entity<IdentityUserClaim<string>>()
                .HasData(claims);

            var userRoles = new IdentityUserRole<string> { UserId = user.Id, RoleId = adminRole.Id };

            builder.Entity<IdentityUserRole<string>>()
                .HasData(userRoles);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.EnableDetailedErrors(true);
            options.EnableSensitiveDataLogging(true);

            base.OnConfiguring(options);
        }
    }
}
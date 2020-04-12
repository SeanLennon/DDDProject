using System;
using System.Threading.Tasks;
using Domain.Entities;
using Identity.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace Identity.Context
{
    public class UserDbContext : IdentityDbContext<User, Role, string>
    {
        protected UserDbContext() { }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }
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
                }
            });
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new RoleMap());

            var noneRole = new Role("None");
            var userRole = new Role("User");
            var adminRole = new Role("Admin");
            var managerRole = new Role( "Manager");

            builder.Entity<Role>()
                .HasData(noneRole, userRole, adminRole, managerRole);

            string email = "seantpd@gmail.com";
            string username = "dev_master";
            string name = "Sean Ono Lennon Pessoa";
            User user = new User(name, username, email);

            builder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = managerRole.Id,
                    UserId = user.Id
                });

            builder.Entity<User>()
                .HasData(user);

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
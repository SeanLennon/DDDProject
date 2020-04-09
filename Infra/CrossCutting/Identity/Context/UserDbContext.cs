using Domain.Entities;
using Identity.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Identity.Context
{
    public class UserDbContext : DbContext
    {
        protected UserDbContext() { }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());

            string email = "seantpd@gmail.com";
            string username = "dev_master";
            string name = "Sean Ono Lennon Pessoa";
            User user = new User(name, username, email);

            builder.Entity<User>()
                .HasData(user);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.EnableDetailedErrors(true);
            options.EnableSensitiveDataLogging(true);
            options.UseNpgsql("Host=localhost; Port=5432; User ID=sean; Password=sean@123; Database=DDDProjectUser;", x =>
            {
                x.MigrationsAssembly("Api");
                x.SetPostgresVersion(0, 1);
                x.UseRelationalNulls(true);
            });

            base.OnConfiguring(options);
        }
    }
}
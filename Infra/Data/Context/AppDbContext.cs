using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
        {
            Database.EnsureCreated();
        }

        // public virtual DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.EnableDetailedErrors(true);
            options.EnableSensitiveDataLogging(true);
            /* options.UseNpgsql("Host=localhost; Port=5432; User ID=sean; Password=sean@123; Database=DDDProjectUser; Integrated Security=true; Pooling=true;", x =>
            {
                x.MigrationsAssembly("Api");
                x.SetPostgresVersion(0, 1);
                x.UseRelationalNulls(true);
            }); */

            base.OnConfiguring(options);
        }
    }
}
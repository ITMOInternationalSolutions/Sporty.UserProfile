using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sporty.UserProfile.Data.Organizations;
using Sporty.UserProfile.Data.Users;

namespace Sporty.UserProfile.Data
{
    public class SportyContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<OrganizationDbModel> Organizations { get; set; }

        public string ConnectionString { get; }

        public SportyContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            ConnectionString = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                configuration["SQLiteConnectionString"]);
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={ConnectionString}");
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationDbModel>()
                .HasMany(o => o.Members);
            modelBuilder.Entity<OrganizationDbModel>()
                .HasMany(o => o.Organizers);
            modelBuilder.Entity<Organizer>()
                .Property(o => o.Id)
                .IsRequired()
                .ValueGeneratedNever();
            modelBuilder.Entity<Member>()
                .Property(m => m.Id)
                .IsRequired()
                .ValueGeneratedNever();
            base.OnModelCreating(modelBuilder);
        }
    }
}
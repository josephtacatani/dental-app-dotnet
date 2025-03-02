using Microsoft.EntityFrameworkCore;
using mydental.domain.Entities;
using mydental.infrastructure.Configurations;

namespace mydental.infrastructure.Data
{
    public class MyDentalDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MyDentalDbContext(DbContextOptions<MyDentalDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}

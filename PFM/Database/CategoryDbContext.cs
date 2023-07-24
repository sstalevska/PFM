using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFM.Database.Entities;
using System.Reflection;
using System.Reflection.Metadata;

namespace PFM.Database
{
    public class CategoryDbContext : DbContext
    {
        public DbSet<CategoryEntity> Categories { get; set; }
        public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options)
        {
        }

        protected CategoryDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryEntity>()
                .HasMany(e => e.splits)
                .WithOne(e => e.category)
                .HasForeignKey(e => e.catcode)
                .IsRequired();
               
            modelBuilder.Entity<CategoryEntity>()
                 .HasMany(e => e.transactions)
                .WithOne(e => e.category)
                .HasForeignKey(e => e.CatCode);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}


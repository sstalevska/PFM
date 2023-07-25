using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;
using System.Reflection;

namespace PFM.Database
{
    public class PfmDbContext : DbContext 
    {
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<SplitEntity> Splits { get; set; }
        public DbSet<CategoryEntity> Categories{ get; set; }



        public PfmDbContext(DbContextOptions<PfmDbContext> options) : base(options)
        {
        }
        protected PfmDbContext()
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

            modelBuilder.Entity<TransactionEntity>()
                   .HasMany(e => e.Splits)
                   .WithOne(e => e.transaction)
                   .HasForeignKey(e => e.transactionid);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

   }
}

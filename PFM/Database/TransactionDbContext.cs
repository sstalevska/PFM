

using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;
using System.Reflection;
using System.Reflection.Metadata;

namespace PFM.Database
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<TransactionEntity> Transactions { get; set; }
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {
        }

        protected TransactionDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransactionEntity>()
                .HasMany(e => e.Splits)
                .WithOne(e => e.transaction)
                .HasForeignKey(e => e.transactionid);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}
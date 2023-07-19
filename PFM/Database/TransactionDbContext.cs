

using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;
using System.Reflection;

namespace PFM.Database
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<TransactionEntity> Transactions { get; set; }
        public TransactionDbContext(DbContextOptions options) : base(options)
        {
        }

        protected TransactionDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}

using Microsoft.EntityFrameworkCore;
using PFM.Database.Entities;
using System.Reflection;

namespace PFM.Database
{
    public class SplitDbContext : DbContext
    {
        public DbSet<SplitEntity> Splits { get; set; }
        public SplitDbContext(DbContextOptions<SplitDbContext> options) : base(options)
        {
        }

        protected SplitDbContext()
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

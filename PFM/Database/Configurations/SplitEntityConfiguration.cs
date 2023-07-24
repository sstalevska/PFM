using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFM.Database.Entities;

namespace PFM.Database.Configurations
{
    public class SplitEntityConfiguration : IEntityTypeConfiguration<SplitEntity>
    {
        public SplitEntityConfiguration() { }
        public void Configure(EntityTypeBuilder<SplitEntity> builder)
        {
            builder.ToTable("splits");
            // primary key
            builder.HasKey(x => x.id);
            // definition of columns
            builder.Property(x => x.id).IsRequired();
            builder.Property(x => x.catcode).IsRequired();
            builder.Property(x => x.amount).IsRequired();
            
        }
    }
}

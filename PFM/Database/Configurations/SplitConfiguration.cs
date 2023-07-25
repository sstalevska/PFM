using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFM.Database.Entities;

namespace PFM.Database.Configurations
{
    public class SplitConfiguration : IEntityTypeConfiguration<SplitEntity>
    {
        public SplitConfiguration() { }
        public void Configure(EntityTypeBuilder<SplitEntity> builder)
        {
            builder.HasKey(x => x.id);
           
            
        }
    }
}

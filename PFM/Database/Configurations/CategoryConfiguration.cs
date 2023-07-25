using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Database.Configurations 
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
       // public CategoryConfiguration() { }
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.HasKey(x => x.code);
            builder.HasMany(x => x.transactions);
            builder.Property(x => x.parentcode).IsRequired(false);
        }
    
        

    }
}

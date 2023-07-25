using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Database.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {

       // public TransactionConfiguration(){}
        

        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.HasKey(x => x.id);
            builder
                .HasOne(x => x.category)
                .WithMany(x => x.transactions)
                .HasForeignKey(x => x.catcode).IsRequired(false);
            builder
                .Property(x => x.currency)
                .HasMaxLength(3);
            builder.HasMany(x => x.splits);
        }
    }
}

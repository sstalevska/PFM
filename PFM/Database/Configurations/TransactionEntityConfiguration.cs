﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFM.Database.Entities;
using PFM.Models;

namespace PFM.Database.Configurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {

        public TransactionEntityTypeConfiguration()
        {

        }

        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");
            // primary key
            builder.HasKey(x => x.Id);
            // definition of columns
            builder.Property(x => x.Id).IsRequired().HasMaxLength(64);
            builder.Property(x => x.BeneficiaryName).HasMaxLength(64);
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Direction).HasConversion<string>().IsRequired(); 
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1024);
            builder.Property(x => x.Currency).IsRequired().HasMaxLength(3);
            builder.Property(x => x.Mcc);
            builder.Property(x => x.Kind).HasConversion<string>().IsRequired();
            builder.Property(x => x.CatCode).HasConversion<string>();
            builder.Property(x => x.Splits).HasConversion<string>();

           // builder.HasMany(x => x.Splits);
            //builder.HasOne(x => x.CatCode);



        }
    }
}

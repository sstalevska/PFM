﻿using PFM.Database.Entities;

namespace PFM.Models
{
    public class Split
    {
        public int id { get; set; }
        public string transactionid { get; set; }
        public TransactionEntity transaction { get; set; } = null!;

        public string catcode { get; set; }
        public CategoryEntity category { get; set; }
        public double amount { get; set; }
    }
}

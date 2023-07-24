using PFM.Models;

namespace PFM.Database.Entities
{
    public class SplitEntity
    {
        public int id { get; set; }
        public string transactionid { get; set; }
        public TransactionEntity transaction { get; set; } = null!;

        public string catcode { get; set; }
        public CategoryEntity category { get; set; }
        public double amount { get; set; }        
    }
}

using Microsoft.Extensions.Hosting;
using PFM.Database.Entities;
using PFM.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string BeneficiaryName { get; set; }
        public DateTime Date { get; set; }
        public Direction Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string Mcc { get; set; }
        public TransactionKind Kind { get; set; }
        public string CatCode { get; set; }
        public CategoryEntity category { get; set; }

        [NotMapped]
        public ICollection<SplitEntity> Splits { get; set; } = new List<SplitEntity>();
    }
}

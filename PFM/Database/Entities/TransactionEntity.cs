using PFM.Models.Enums;
using PFM.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.Database.Entities
{
    public class TransactionEntity
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

        public string? CatCode { get; set; } = null;
        public CategoryEntity category { get; set; }
        public List<SplitEntity> Splits { get; set; }

    }
}
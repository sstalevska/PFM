using PFM.Models.Enums;
using PFM.Models;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.Database.Entities
{
    public class TransactionEntity
    {
        public string id { get; set; }
        public string beneficiaryname { get; set; }
        public DateTime date { get; set; }
        public Direction direction { get; set; }
        public double amount { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
        public string mcc { get; set; }
        public TransactionKind kind { get; set; }

        public string? catcode { get; set; } = null;
        public CategoryEntity category { get; set; }
        public List<SplitEntity> splits { get; set; }

    }
}
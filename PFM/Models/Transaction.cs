using Microsoft.Extensions.Hosting;
using PFM.Database.Entities;
using PFM.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.Models
{
    public class Transaction
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
        public string? CatCode { get; set; } = null;
        public CategoryEntity category { get; set; }
        public List<Split> splits { get; set; }


        
    }
}

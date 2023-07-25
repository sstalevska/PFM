using PFM.Models.Enums;
using PFM.Models;
using System.ComponentModel.DataAnnotations;

namespace PFM.Commands
{
    public class CategorizeTransactionCommand
    {


        [Required]
        public string Id { get; set; }

        public string BeneficiaryName { get; set; }
        public DateTime Date { get; set; }
        public Direction Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string Mcc { get; set; }
        public TransactionKind Kind { get; set; }

     //   [Required]
        public Category CatCode { get; set; }
        public Split Splits { get; set; }
    }
}

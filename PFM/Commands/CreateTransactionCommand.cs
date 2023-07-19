using PFM.Models.Enums;
using PFM.Models;
using System.ComponentModel.DataAnnotations;

namespace PFM.Commands
{
    public class CreateTransactionCommand
    {
     

        [Required]
        public string Id { get; set; }
      
        public string BeneficiaryName { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Direction Direction { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Description { get; set; }
        [Required]
        public string Currency { get; set; }
        public string Mcc { get; set; }
        [Required]
        public TransactionKind Kind { get; set; }
        public string CatCode { get; set; }
        public Split Splits { get; set; }
    }
    
}

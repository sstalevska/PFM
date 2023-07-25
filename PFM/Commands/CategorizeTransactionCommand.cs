using PFM.Models.Enums;
using PFM.Models;
using System.ComponentModel.DataAnnotations;

namespace PFM.Commands
{
    public class CategorizeTransactionCommand
    {


        [Required]
        public string id { get; set; }

        public string beneficiaryname { get; set; }
        public DateTime date { get; set; }
        public Direction direction { get; set; }
        public double amount { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
        public string mcc { get; set; }
        public TransactionKind kind { get; set; }

     //   [Required]
        public Category catcode { get; set; }
    }
}

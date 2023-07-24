using PFM.Models;
using PFM.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace PFM.Commands
{
    public class TransactionCSVCommand
    {

        [Required]
        public string id { get; set; }
        public string beneficiaryname { get; set; }
        [Required]
        public DateTime date { get; set; }
        [Required]
        public Direction direction { get; set; }
        [Required]
        public double amount { get; set; }
        public string description { get; set; }
        [Required]
        public string currency { get; set; }
        public string mcc { get; set; }
        public TransactionKind kind { get; set; }
        public Category catcode { get; set; }
        public Split splits { get; set; }
    }
}

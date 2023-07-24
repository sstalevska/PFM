using Microsoft.Extensions.Hosting;
using PFM.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.Models
{
    public class Category
    {
        public string code { get; set; }
        public string name { get; set; }
        public string parentcode { get; set; }

        [NotMapped]
        public IEnumerable<SplitEntity> splits { get; set; } = new List<SplitEntity>();
        [NotMapped]
        public IEnumerable<TransactionEntity> transactions { get; set; } = new List<TransactionEntity>();

    }
}

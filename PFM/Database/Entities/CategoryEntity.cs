

using Microsoft.Extensions.Hosting;
using PFM.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.Database.Entities
{
    public class CategoryEntity
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public string parentcode { get; set; }

        public List<SplitEntity> splits { get; set; }
        public List<TransactionEntity> transactions { get; set; }
    }
}

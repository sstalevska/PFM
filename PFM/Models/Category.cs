using Microsoft.Extensions.Hosting;
using PFM.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFM.Models
{
    public class Category
    {
        [Key]
        public string code { get; set; }
        public string name { get; set; }
        public string parentcode { get; set; }

        public List<Transaction> transactions { get; set; }

    }
}

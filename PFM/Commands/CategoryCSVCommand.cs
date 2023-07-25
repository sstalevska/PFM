using System.ComponentModel.DataAnnotations;

namespace PFM.Commands
{
    public class CategoryCSVCommand
    {
        [Required]
        [Key]
        public string code { get; set; }
        [Required]
        public string name { get; set; }
        public string parentcode { get; set; }
    }
}

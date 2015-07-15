using System.ComponentModel.DataAnnotations;

namespace CashCard.Models
{
    public class RegularGroup
    {
        public int Id { get; set; }
        [Required]
        public string AccountNo { get; set; }
        [Required]
        public string AccountDesription { get; set; }
        public GroupType GroupType { get; set; }
    }
}
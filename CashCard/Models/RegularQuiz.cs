using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace CashCard.Models
{
    public class RegularQuiz
    {
        public int Id { get; set; }
        [Required]
        public string Quiz { get; set; }
        [ForeignKey("RegularGroupId")]
        public virtual RegularGroup RegularGroup { get; set; }
        public int RegularGroupId { get; set; }
        public RegularType RegularType { get; set; }
        public string Info { get; set; }
        
    }
}
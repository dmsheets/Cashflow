using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace CashCard.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        [Required]
        public string Info { get; set; }
        [ForeignKey("QuizGroupId")]
        public virtual QuizGroup QuizGroup { get; set; }
        public int? QuizGroupId { get; set; }
        public CostCenter CostCenter { get; set; }
        public string Note1Label { get; set; }
        public string Note2Label { get; set; }
        public string Note3Label { get; set; }
        public bool RequiredAll { get; set; }
        
        
    }
}
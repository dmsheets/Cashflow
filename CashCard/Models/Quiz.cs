using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace CashCard.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Quiz")]
        public string Info { get; set; }
        [ForeignKey("QuizGroupId")]
        [Display(Name = "GL Account")]
        public virtual QuizGroup QuizGroup { get; set; }
        public int? QuizGroupId { get; set; }
        [Display(Name = "Cost Center")]
        public CostCenter CostCenter { get; set; }
        [Display(Name = "Label 1")]
        public string Note1Label { get; set; }
        [Display(Name = "Label 2")]
        public string Note2Label { get; set; }
        [Display(Name = "Label 3")]
        public string Note3Label { get; set; }
        public bool RequiredAll { get; set; }
        
        
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public class RegularDetail
    {
        public int Id { get; set; }
        [ForeignKey("RegularDetailQuizId")]
        public virtual RegularDetailQuiz RegularDetailQuiz { get; set; }
        public int RegularDetailQuizId { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public int Amount { get; set; }
        [ForeignKey("CashFlowId")]
        public CashOutRegular CashOutRegular { get; set; }
        public int CashFlowId { get; set; }
    }
}
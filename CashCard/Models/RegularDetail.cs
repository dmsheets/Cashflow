using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public class RegularDetail
    {
        public int Id { get; set; }
        [ForeignKey("RegularQuizId")]
        public virtual RegularQuiz RegularDetailQuiz { get; set; }
        public int RegularQuizId { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public int Amount { get; set; }
        public int Count { get; set; }
        public int SubTotal { get; protected set; }
        [ForeignKey("CashFlowId")]
        public CashOutRegular CashOutRegular { get; set; }
        public int CashFlowId { get; set; }

        public void GetSubTotal()
        {
            SubTotal = Amount*Count;
        }
    }
}
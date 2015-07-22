using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public class RegularDetail
    {
        public int Id { get; set; }
        [ForeignKey("RegularQuizId")]
        public virtual RegularQuiz RegularDetailQuiz { get; set; }
        public int? RegularQuizId { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Qty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int SubTotal { get; protected set; }
        [ForeignKey("CashFlowId")]
        public CashOutRegular CashOutRegular { get; set; }
        public int? CashFlowId { get; set; }

        public void SetSubTotal()
        {
            SubTotal = Amount*Qty;
        }
    }
}
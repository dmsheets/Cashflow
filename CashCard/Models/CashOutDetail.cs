using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public class CashOutDetail
    {
        public int Id { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }
        public int? QuizId { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DateInfo { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Qty { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int SubTotal { get; protected set; }
        [ForeignKey("CashOutId")]
        public CashOut CashOut { get; set; }
        public int? CashOutId { get; set; }

        public void SetSubTotal()
        {
            SubTotal = Amount*Qty;
        }
    }
}
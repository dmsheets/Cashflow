using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public class IrregularDetail
    {
        public int Id { get; set; }
        public IrregularType IrregularType { get; set; }
        public string FlightNo { get; set; }
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime FlightDate { get; set; }
        public string FromTo { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Qty { get; set; }
        public string Note { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int SubTotal { get; protected set; }
        [ForeignKey("CashFlowId")]
        public CashOutIrregular CashOutIrregular { get; set; }
        public int? CashFlowId { get; set; }
        public void SetSubTotal()
        {
            SubTotal = Amount * Qty;
        }


    }
}
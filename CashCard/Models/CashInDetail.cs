using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public class CashInDetail
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Amount { get; set; }
        public string Note { get; set; }
        [ForeignKey("CashFlowId")]
        public CashIn CashIn { get; set; }
        public int? CashFlowId { get; set; }

    }
}
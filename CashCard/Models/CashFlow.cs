using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public abstract class CashFlow
    {
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public StateCashFlow State { get; set; }
        public string LogNote { get; set; }
        [ForeignKey("CutOffId")]
        public virtual CutOff CutOff { get; set; }
        public int CutOffId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        [ForeignKey("SuperVisorId")]
        public ApplicationUser SuperVisor { get; set; }
        public string SuperVisorId { get; set; }
        public int Total { get; protected set; }

        public abstract void SetTotal();

    }
}
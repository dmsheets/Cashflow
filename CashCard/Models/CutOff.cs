using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CashCard.Models
{
    public class CutOff
    {
        public CutOff()
        {
            DateStart = DateTime.Now;
            //DateEnd = DateTime.Now;
        }
        public int Id { get; set; }
        public StateCutOff State { get;protected set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DateStart { get; protected set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DateEnd { get; protected set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        public int? BranchId { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int PreviousBallance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int EndBallance { get; protected set; }
        public string Note { get; set; }
        public virtual ICollection<CashCard> CashCards { get; set; }

        public void SetEndState()
        {
            if (State == StateCutOff.Close)
                throw new Exception("This date had been Closed");
            State = StateCutOff.Close;
            DateEnd = DateTime.Now;
        }
        public void SetEndBallance()
        {
            int total = PreviousBallance;
            foreach (var c in CashCards)
            {
                if (c.State != StateCashCard.Reject)
                {
                    if (c is CashIn)
                        total += c.Total;
                    else
                        total -= c.Total;
                }
            }
            EndBallance = total;
          
        }
    }
}
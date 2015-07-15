using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CashCard.Models
{
    public class CutOff
    {
        public int Id { get; set; }
        public StateCutOff State { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        public int BranchId { get; set; }
        public int PreviousBallance { get; set; }
        public int EndBallance { get; set; }
        public string Note { get; set; }
        public virtual ICollection<CashFlow> CashFlows { get; set; } 


    }

    public abstract class CashFlow
    {
        public int Id { get; set; }
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
        public int Total { get; protected set; }

        public abstract void SetTotal();

    }

    public class CashIn:CashFlow
    {
        public override void SetTotal()
        {
            throw new NotImplementedException();
        }
    }

    public class CashOutRegular : CashFlow
    {
        public RegularType RegularType { get; set; }
        public ICollection<RegularDetail> RegularDetails { get; set; }

        public override void SetTotal()
        {
            int total = 0;
            foreach (var x in RegularDetails)
            {
                total += x.SubTotal;


            }
            base.Total = total;
        }
    }

    public class CashOutIregular : CashFlow
    {
        public override void SetTotal()
        {
            throw new NotImplementedException();
        }
    }
}
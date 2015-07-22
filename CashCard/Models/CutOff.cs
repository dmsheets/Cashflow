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
        public int Id { get; set; }
        public StateCutOff State { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        public int? BranchId { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int PreviousBallance { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int EndBallance { get; set; }
        public string Note { get; set; }
        public virtual ICollection<CashFlow> CashFlows { get; set; } 


    }
}
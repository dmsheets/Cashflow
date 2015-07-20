using System.Collections.Generic;

namespace CashCard.Models
{
    public class CashOutRegular : CashFlow
    {
        public RegularType RegularType { get; set; }
        public virtual ICollection<RegularDetail> RegularDetails { get; set; }

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
}
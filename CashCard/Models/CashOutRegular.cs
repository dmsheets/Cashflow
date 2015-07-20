using System.Collections.Generic;

namespace CashCard.Models
{
    public class CashOutRegular : CashFlow
    {
        private IList<RegularDetail> _regularDetails; 
        public CashOutRegular()
        {
            _regularDetails = new List<RegularDetail>();

        }
        public RegularType RegularType { get; set; }

        public virtual IList<RegularDetail> RegularDetails
        {
            get { return _regularDetails; }
            set { _regularDetails = value; }
        }

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
using System;
using System.Collections.Generic;

namespace CashCard.Models
{
    public class CashOut : CashCard
    {
        private IList<CashOutDetail> _regularDetails; 
        public CashOut()
        {
            _regularDetails = new List<CashOutDetail>();

        }
        public CostCenter CostCenter { get; set; }
        public TypeOut TypeOut { get; set; }

        public virtual IList<CashOutDetail> RegularDetails
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
using System;
using System.Collections;
using System.Collections.Generic;

namespace CashCard.Models
{
    public class CashOutIrregular : CashFlow
    {
        private IList<IrregularDetail> _irregularDetails;

        public CashOutIrregular()
        {
            _irregularDetails = new List<IrregularDetail>();
        }

        public virtual IList<IrregularDetail> IrregularDetails
        {
            get { return _irregularDetails; }
            set { _irregularDetails = value; }
        }
        public override void SetTotal()
        {
            int total = 0;
            foreach (var x in IrregularDetails)
            {
                total += x.SubTotal;


            }
            base.Total = total;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashCard.Models
{
    public class CashIn:CashFlow
    {
        private IList<CashInDetail> _cashInDetails;
        public CashIn()
        {
            _cashInDetails = new List<CashInDetail>();
        }
        public override void SetTotal()
        {
            int total = CashInDetails.Sum(x => x.Amount);
            Total = total;
        }

        public virtual IList<CashInDetail> CashInDetails
        {
            get { return _cashInDetails; }
            set { _cashInDetails = value; }
        }
    }
}
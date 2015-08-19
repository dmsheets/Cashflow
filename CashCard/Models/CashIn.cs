using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace CashCard.Models
{
    public class CashIn:CashCard
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

    public class Kendaraan
    {
        public int Id { get; set; }
         [Remote("CheckNo", "Kendaraan")]
         [Display(Name = "No Pol")]
        public string NoKendaraan { get; set; }
          [Display(Name = "Jenis Kendaraan")]
        public string JenisKendaraan { get; set; }
           [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
           [Display(Name = "Eff Start Date")]
        public DateTime? EffStartDate { get; set; }
           [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
           [Display(Name = "Eff End Date")]
        public DateTime? EffEndDate { get; set; }
        public string Cc { get; set; }
          [Display(Name = "Merk")]
        public string MerkKendaraan { get; set; }
          [Display(Name = "Tahun")]
        public string ThnPembuatan { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CashCard.Models
{
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
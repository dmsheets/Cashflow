using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CashCard.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [Required]
        [Remote("CheckName", "Branch")]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int PengeluaranRegular { get; set; }
    }
}
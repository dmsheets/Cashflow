using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CashCard.Models
{
    public class RegularGroup
    {
        public int Id { get; set; }
        [Required]
        [Remote("CheckName", "RegularGroup")]
        public string AccountNo { get; set; }
        [Required]
        public string AccountDesription { get; set; }
        public GroupType GroupType { get; set; }
        public string Info { get; set; }
    }
}
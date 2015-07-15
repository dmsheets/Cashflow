﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace CashCard.Models
{
    public class RegularDetailQuiz
    {
        public int Id { get; set; }
        [Required]
        public string Quiz { get; set; }
        [ForeignKey("RegularGroupId")]
        public RegularGroup RegularGroup { get; set; }
        public int RegularGroupId { get; set; }
        public RegularType RegularType { get; set; }
        public bool Active { get; set; }
    }
}
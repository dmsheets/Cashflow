using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashCard.Models
{
    public class DataTablesJson
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IList<string[]> data { get; set; }
        public string error { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
}
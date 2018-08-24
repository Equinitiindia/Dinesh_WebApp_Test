using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Type { get; set; }
        public string Summary { get; set; }
        public decimal Amount { get; set; }
        public string PostingDate { get; set; }
        public bool IsCleared { get; set; }
        public string ClearedDate { get; set; }
    }
}
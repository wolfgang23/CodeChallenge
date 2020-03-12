using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestRest.Models
{
    public class Invoice
    {
        public int invoiceId { get; set; }
        public List<Transaction> transactions { get; set; }

        public Invoice() { }
    }
}

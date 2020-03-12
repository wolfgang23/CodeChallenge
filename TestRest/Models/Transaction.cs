using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestRest.Models
{
    public class Transaction
    {
        public int transactionId { get; set; }
        public DateTime transactDate { get; set; }
        public float transactAmount { get; set; }
        public string transactDesc { get; set; }
        public TransactionStatus transactStatus { get; set; }

        public Transaction() { }

        public Transaction(int tId, DateTime date, float amount, string desc)
        {
            transactionId = tId;
            transactDate = date;
            transactAmount = amount;
            transactDesc = desc;
            transactStatus = TransactionStatus.UNBILLED;
        }

    }

    public enum TransactionStatus
    {
        UNBILLED = 1,
        BILLED = 2,
        PAID = 3
    }
}

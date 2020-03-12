using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRest.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//adding comments ...
namespace TestRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
       
        //GET:      api/commands
        /*[HttpGet]
        public ActionResult<IEnumerable<string>> Getit()
        {
            return new string[] { "this","is","test" };
        }
        */
        private readonly MyDbContext _context;

        public CommandsController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var tran = await _context.Transaction.ToArrayAsync();

            var response = tran.Select(t => new
            {
                transactionid = t.transactionId,
                transactiondate = t.transactDate,
                transactiondesc = t.transactDesc,
                transactionamount = t.transactAmount,
                transactionstatus = t.transactStatus,
            });
            

            return Ok(tran);
        }

        [HttpGet("getinvoices")]
        public async Task<IActionResult> GetAllInvoices()
        {
            var tran = await _context.Invoices.Include(t => t.transactions).ToArrayAsync();

            return Ok(tran);
        }


        [HttpGet("{dateini}/{dateend}")]
        public async Task<IActionResult> CreateInvoice(string dateini , string dateend)
        {
            DateTime init = DateTime.Parse(dateini);
            DateTime end = DateTime.Parse(dateend);

            var tran = await _context.Transaction.ToArrayAsync();
            //get transactions that match the given range
            var invoice = from r in tran where r.transactDate >= init && r.transactDate <= end select r;
            //Change the transaction status of the transactions that were on the date range
            foreach (var inv in invoice)
            {
                inv.transactStatus = TransactionStatus.BILLED;
                _context.Entry(inv).State = EntityState.Modified;
                _context.SaveChanges();
            }
            //Add the new invoice
            var allinvoices = await _context.Invoices.Include(t => t.transactions).ToArrayAsync();
            int invoiceid = 1;
            if (allinvoices.Count() > 0)
            {
                //increment the invoiceid
                invoiceid = allinvoices[allinvoices.Count() - 1].invoiceId;
                invoiceid += 1;
            }
            Invoice newinvoice = new Invoice();
            newinvoice.transactions = new List<Transaction>();
            newinvoice.transactions = invoice.ToList();
            newinvoice.invoiceId = invoiceid;
            _context.Invoices.Add(newinvoice);
            _context.SaveChanges();

            return Ok(invoice);
        }

        //POST:     api/commands
        [HttpPost]
        [Route("createtran")]
        public ActionResult<Transaction> CreateTransaction([FromQuery]string date, [FromQuery]string desc, [FromQuery]string amount)//(Transaction transact)
        {
            try
            {
                //test
                var transact = new Transaction();
                transact.transactDate = DateTime.Parse(date);
                transact.transactDesc = desc;
                transact.transactAmount = float.Parse(amount);
                //test
                var alltransactions =  _context.Transaction.ToArray();
                int tranid = 1;
                if (alltransactions.Count() > 0)
                {
                    //increment the invoiceid
                    tranid = alltransactions[alltransactions.Count() - 1].transactionId;
                    tranid += 1;
                }
                //asign unique id
                transact.transactionId = tranid;
                //Add the transaction to the dbcontext
                _context.Transaction.Add(transact);
                _context.SaveChanges();

                return CreatedAtAction("Get", transact);
            }
            catch(Exception ex)
            {
                JsonResult jres = new JsonResult("Error: "+ex.Message);
                return jres;
            }
        }

        //PUT:      api/commands/n
        [HttpPut("{id:int}")]
        public ActionResult UpdateTransaction(int id, Transaction transact)
        {
            if (id != transact.transactionId)
            {
                return BadRequest();
            }
            //updates the given transaction if the id matches
            _context.Entry(transact).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(transact);
        }

        //PUT:      api/commands/n
        [HttpPut("invoice/{invoiceid}")]
        public async Task<IActionResult> SetPaidInvoice(int invoiceid)
        {
            var invoices = await _context.Invoices.Include(t=> t.transactions).ToArrayAsync();
            //search for the invoice that needs to be modified
            foreach (var inv in invoices)
            {
               if(inv.invoiceId == (invoiceid))
                {
                    foreach(var tran in inv.transactions)
                    {
                        //change the status of the transactions that were linked to the invoice
                        tran.transactStatus = TransactionStatus.PAID;
                    }
                    _context.Entry(inv).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok(inv);
                }

            }

            return BadRequest();
        }

    }
}

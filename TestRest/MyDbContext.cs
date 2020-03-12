using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestRest.Models;

namespace TestRest
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options) { }

        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
    }
}

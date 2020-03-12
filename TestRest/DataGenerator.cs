using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestRest
{
    public class DataGenerator
    {
        public static void AddTestData(MyDbContext context)
        {
            Models.Transaction t1 = new Models.Transaction(1, System.DateTime.Now, 12, "test");
            Models.Transaction t2 = new Models.Transaction(2, System.DateTime.Now, 134, "test2");
            Models.Transaction t3 = new Models.Transaction(3, System.DateTime.Now, 6556, "test3");
            context.Add(t1);
            context.Add(t2);
            context.Add(t3);

            context.SaveChanges();
        }
    }
}

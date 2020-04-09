using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TestRest.Models;

namespace ApiTest
{
    public class CommandControllerUnitTest
    {
        [Fact]
        public async Task TestGet()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/Commands");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestGetAllInvoices()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/Commands/getinvoices");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestCreateInvoice()
        {
            using (var client = new TestClientProvider().Client)
            {
                var dateini = "2020-01-01";
                var dateend = "2020-01-15";
                var response = await client.GetAsync("/api/Commands/"+dateini+"/"+dateend);

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestCreateTransaction()
        {
            using (var client = new TestClientProvider().Client)
            {
                var parameters = new Dictionary<string, string>();
                //parameters.Add("date","2020-01-01");
                //parameters.Add("desc", "test");
                //parameters.Add("amount","100");
                //var encodedContent = new FormUrlEncodedContent(parameters);
                var jsonString = JsonConvert.SerializeObject(new { date = "2020-01-01", desc = "test" , amount = "100"});
                //var jsonString = "{\"date\":'2020-01-01',\"desc\":'test',\"amount\":'100'}";
                var encodedContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/Commands/createtran",encodedContent);

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task TestUpdateTran()
        {
            using (var client = new TestClientProvider().Client)
            {
                var jsonString = JsonConvert.SerializeObject(new Transaction(1,new DateTime(2020,1,1),100,"test"));
                var encodedContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PutAsync("/api/Commands/1",encodedContent);

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task TestSetPaidInvoice()
        {
            using (var client = new TestClientProvider().Client)
            {
               
                var response = await client.PutAsync("/api/Commands/invoice/1",null);

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}

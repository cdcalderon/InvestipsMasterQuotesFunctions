using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InvestipsMasterQuotesFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace InvestipsMasterQuotesFunctions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://enigmatic-waters-56889.herokuapp.com/");
            _httpClient.Timeout = new TimeSpan(0,0,45);

            var response = await _httpClient.GetAsync("api/udf/allsymbols");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var symbols = JsonConvert.DeserializeObject<IEnumerable<string>>(content);

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            if (name == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data?.name;
            }

            using (var db = new InvestipsQuotesContext())
            {
                db.Quotes.Add(new Quote { Symbol = "AAPL", Open = 100, High = 103, Low = 99, Close = 102, TimeStampDateTime = DateTime.Now});
                await db.SaveChangesAsync();
                log.Info("Quote Created");
            }

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }
}

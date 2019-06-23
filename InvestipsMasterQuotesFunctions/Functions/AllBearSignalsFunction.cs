using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Investips.Data.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace InvestipsMasterQuotesFunctions.Functions
{
    public static class AllBearSignalsFunction
    {
        [FunctionName("AllBearSignalsFunction")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestMessage req, TraceWriter log)
        {

            List<Quote> quotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(q => q.IsSuperGapBear == true)
                    .OrderByDescending(q => q.TimeStampDateTime).ToList();
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(quotes.Select(q => q.Symbol).Distinct().ToList()), Encoding.UTF8,
                    "application/json")
            };

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Investips.Data.Models;
using InvestipsMasterQuotesFunctions.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace InvestipsMasterQuotesFunctions.Functions
{
    public static class BullEightSignalsFunction
    {
        [FunctionName("BullEightSignalsFunction")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestMessage req, TraceWriter log)
        {

            string from = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => String.Compare(q.Key, "from", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            string to = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => String.Compare(q.Key, "to", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            string symbol = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => String.Compare(q.Key, "symbol", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            string resolution = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => String.Compare(q.Key, "resolution", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;
            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => String.Compare(q.Key, "name", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;

            List<Quote> quotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(
                    q => q.IsBullEight45Degreed && q.Symbol == symbol
                ).ToList();
            }
            
            if (quotes.Count > 0)
            {
                quotes.Add(quotes.Last()); // Temporary POC fix for issue displaying last item in UI
                var ids = Enumerable.Range(0, quotes.Count() - 1).ToList();
                var times = quotes.Select(x => Convert.ToInt64(ToUnixTimeStamp(x.TimeStampDateTime)))
                    .ToList();
                var colors = quotes.Select(x => "green").ToList();
                var texts = quotes.Select(x => "BullBigEight").ToList();
                var labels = quotes.Select(x => "B8").ToList();
                var labelFontColors = quotes.Select(x => "black").ToList();
                var minSizes = quotes.Select(x => 20).ToList();

                var marks = new SignalMarks()
                {
                    id = ids,
                    time = times,
                    color = colors,
                    text = texts,
                    label = labels,
                    labelFontColor = labelFontColors,
                    minSize = minSizes
                };

                var jsonToReturn = JsonConvert.SerializeObject(marks);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
                };
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new SignalMarks()), Encoding.UTF8,
                    "application/json")
            };

        }

        public static double ToUnixTimeStamp(DateTime dtime)
        {
            var dd = (dtime - new DateTime(1970, 1, 1));
            var dd2 = (dtime - new DateTime(1970, 1, 1).ToLocalTime());

            var tt1 = dd.TotalSeconds;
            var tt2 = dd2.TotalSeconds;
            return tt1;
        }

        public static DateTime FromUnixTimeStamp(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}

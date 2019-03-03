using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Investips.Data.Models;
using InvestipsMasterQuotesFunctions.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace InvestipsMasterQuotesFunctions.Functions
{
   
    public static class GapSignalsFunction
    {
        [FunctionName("GapSignalsFunction")]
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

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); //from 1970/1/1 00:00:00 to now

            var yesterday = DateTime.UtcNow.AddDays(-2);

            TimeSpan result = yesterday.Subtract(dt);

            int seconds = Convert.ToInt32(result.TotalSeconds);


            List<Quote> quotes = new List<Quote>();
            var matchedQuotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(q => q.Symbol == symbol &&
                                              (q.IsMacdCrossingHorizontalUp || q.IsPriceCrossMovAvg30Up ||
                                               q.IsStoch145Cossing25Up))
                    .OrderBy(q => q.TimeStampDateTime).ToList();
                for (int i = 0; i < quotes.Count; i++)
                {

                    var quote = quotes[i];
                    var isMacd = quote.IsMacdCrossingHorizontalUp ? 1 : 0;
                    var isMovAvg30 = quote.IsPriceCrossMovAvg30Up ? 1 : 0;
                    var isStoch145 = quote.IsStoch145Cossing25Up ? 1 : 0;
                    var markCount = isMacd + isMovAvg30 + isStoch145;
                    
                }

                log.Info("Quote matched Done");
            }

            if (matchedQuotes.Count > 0)
            {
                var ids = Enumerable.Range(0, matchedQuotes.Count() - 1).ToList();
                var times = matchedQuotes.Select(x => Convert.ToInt64(ToUnixTimeStamp(x.TimeStampDateTime)))
                    .ToList();
                var colors = matchedQuotes.Select(x => "green").ToList();
                var texts = matchedQuotes.Select(x => "3 Green Arrows").ToList();
                var labels = matchedQuotes.Select(x => "G").ToList();
                var labelFontColors = matchedQuotes.Select(x => "black").ToList();
                var minSizes = matchedQuotes.Select(x => 20).ToList();

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
            return (dtime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public static DateTime FromUnixTimeStamp(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}

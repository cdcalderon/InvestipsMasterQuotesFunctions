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
    public static class CombinedBearSignalFunctions
    {
        [FunctionName("BearAllSignalsFunction")]
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
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;
            
            var marks1 = GetBearGap(symbol);

            var allColors = new List<SignalMarks>() { marks1 }.Where(x => x.color != null).SelectMany(m => m.color);
            var allLabels = new List<SignalMarks>() { marks1 }.Where(x => x.label != null).SelectMany(m => m.label);
            var allLabelFontColor = new List<SignalMarks>() { marks1 }.Where(x => x.labelFontColor != null).SelectMany(m => m.labelFontColor);
            var allMinSizes = new List<SignalMarks>() { marks1 }.Where(x => x.minSize != null).SelectMany(m => m.minSize);
            var allTexts = new List<SignalMarks>() { marks1 }.Where(x => x.text != null).SelectMany(m => m.text);
            var allTimes = new List<SignalMarks>() { marks1 }.Where(x => x.time != null).SelectMany(m => m.time);

            if (allTimes.Any())
            {
                var ids = Enumerable.Range(1, allTimes.Count()).ToList();
                var times = allTimes.ToList();
                var colors = allColors.ToList();
                var texts = allTexts.ToList();
                var labels = allLabels.ToList();
                var labelFontColors = allLabelFontColor.ToList();
                var minSizes = allMinSizes.ToList();

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

        

        public static SignalMarks GetBearGap(string symbol)
        {
            var marks = new SignalMarks();
            List<Quote> quotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(q => q.IsSuperGapBear == true && q.Symbol == symbol).ToList();
            }


            if (quotes.Count > 0)
            {
                quotes.Add(quotes.Last()); // Temporary POC fix for issue displaying last item in UI
                var ids = Enumerable.Range(1, quotes.Count()).ToList();
                var times = quotes.Select(x => Convert.ToInt64(ToUnixTimeStamp(x.TimeStampDateTime)))
                    .ToList();
                var colors = quotes.Select(x => "red").ToList();
                var texts = quotes.Select(x => "Super Gaps").ToList();
                var labels = quotes.Select(x => "GP").ToList();
                var labelFontColors = quotes.Select(x => "white").ToList();
                var minSizes = quotes.Select(x => 20).ToList();

                marks = new SignalMarks()
                {
                    id = ids,
                    time = times,
                    color = colors,
                    text = texts,
                    label = labels,
                    labelFontColor = labelFontColors,
                    minSize = minSizes
                };
            }

            return marks;
        }

       
    }
}


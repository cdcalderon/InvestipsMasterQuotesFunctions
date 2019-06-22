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
    public static class CombinedSignalsFunction
    {
        [FunctionName("BullAllSignalsFunction")]
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

            var marks1 = GetBullBigEightMarks(symbol);
            var marks2 = GetBullThreeArrowsMarks(symbol);
            var marks3 = GetBullGap(symbol);
            var marks4 = GetBullStoch307(symbol);

            var allColors = new List<SignalMarks>() { marks1, marks2, marks3, marks4 }.Where(x => x.color != null).SelectMany(m => m.color);
            var allLabels = new List<SignalMarks>() { marks1, marks2, marks3, marks4 }.Where(x => x.label != null).SelectMany(m => m.label);
            var allLabelFontColor = new List<SignalMarks>() { marks1, marks2, marks3, marks4 }.Where(x => x.labelFontColor != null).SelectMany(m => m.labelFontColor);
            var allMinSizes = new List<SignalMarks>() { marks1, marks2, marks3, marks4 }.Where(x => x.minSize != null).SelectMany(m => m.minSize);
            var allTexts = new List<SignalMarks>() { marks1, marks2, marks3, marks4 }.Where(x => x.text != null).SelectMany(m => m.text);
            var allTimes = new List<SignalMarks>() { marks1, marks2, marks3, marks4 }.Where(x => x.time != null).SelectMany(m => m.time);

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

        public static SignalMarks GetBullStoch307(string symbol)
        {
            var marks = new SignalMarks();
            List<Quote> quotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(
                    q => q.IsStoch101Cossing20Up &&
                         q.IsPriceCrossMovAvg7Up &&
                         q.IsMovingAvg30PointingUp &&
                         q.Symbol == symbol
                ).ToList();
            }

            
            if (quotes.Count > 0)
            {
                quotes.Add(quotes.Last()); // Temporary POC fix for issue displaying last item in UI
                var ids = Enumerable.Range(0, quotes.Count() - 1).ToList();
                var times = quotes.Select(x => Convert.ToInt64(ToUnixTimeStamp(x.TimeStampDateTime)))
                    .ToList();
                var colors = quotes.Select(x => "yellow").ToList();
                var texts = quotes.Select(x => "STOCH307").ToList();
                var labels = quotes.Select(x => "ST").ToList();
                var labelFontColors = quotes.Select(x => "black").ToList();
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

        public static SignalMarks GetBullGap(string symbol)
        {
            var marks = new SignalMarks();
            List<Quote> quotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(q => q.IsSuperGap == true && q.Symbol == symbol).ToList();
            }

           
            if (quotes.Count > 0)
            {
                quotes.Add(quotes.Last()); // Temporary POC fix for issue displaying last item in UI
                var ids = Enumerable.Range(1, quotes.Count()).ToList();
                var times = quotes.Select(x => Convert.ToInt64(ToUnixTimeStamp(x.TimeStampDateTime)))
                    .ToList();
                var colors = quotes.Select(x => "blue").ToList();
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

        public static SignalMarks GetBullThreeArrowsMarks(string symbol)
        {
            var marks = new SignalMarks();
            List<Quote> quotes = new List<Quote>();
            var matchedQuotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(q => q.Symbol == symbol &&
                                              (q.IsMacdCrossingHorizontalUp || q.IsPriceCrossMovAvg30Up || q.IsStoch145Cossing25Up))
                                              .OrderBy(q => q.TimeStampDateTime).ToList();
                for (int i = 0; i < quotes.Count; i++)
                {

                    var quote = quotes[i];
                    var isMacd = quote.IsMacdCrossingHorizontalUp ? 1 : 0;
                    var isMovAvg30 = quote.IsPriceCrossMovAvg30Up ? 1 : 0;
                    var isStoch145 = quote.IsStoch145Cossing25Up ? 1 : 0;
                    var markCount = isMacd + isMovAvg30 + isStoch145;

                    if (quote.IsStoch14505PointingUp)
                    {
                        if (quote.IsMacdCrossingHorizontalUp && quote.IsPriceCrossMovAvg30Up &&
                            quote.IsStoch145Cossing25Up)
                        {
                            matchedQuotes.Add(quote);
                        }
                        //if (quote.IsMacdCrossingHorizontalUp && PreviousTwoAreInRange(i, quote, quotes, markCount))
                        //{
                        //    matchedQuotes.Add(quote);
                        //}
                        //if (quote.IsPriceCrossMovAvg30Up && PreviousTwoAreInRange(i, quote, quotes, markCount))
                        //{
                        //    matchedQuotes.Add(quote);
                        //}
                        //if (quote.IsStoch145Cossing25Up && PreviousTwoAreInRange(i, quote, quotes, markCount))
                        //{
                        //    matchedQuotes.Add(quote);
                        //}
                    }
                }
                
            }

            if (matchedQuotes.Count > 0)
            {
                var ids = Enumerable.Range(0, matchedQuotes.Count() - 1).ToList();
                var times = matchedQuotes.Select(x => Convert.ToInt64(ToUnixTimeStamp(x.TimeStampDateTime))).ToList();
                var colors = matchedQuotes.Select(x => "green").ToList();
                var texts = matchedQuotes.Select(x => "3 Green Arrows").ToList();
                var labels = matchedQuotes.Select(x => "G").ToList();
                var labelFontColors = matchedQuotes.Select(x => "black").ToList();
                var minSizes = matchedQuotes.Select(x => 20).ToList();

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

        public static SignalMarks GetBullBigEightMarks(string symbol)
        {
            var marks = new SignalMarks();
            List<Quote> quotes = new List<Quote>();
            using (var db = new InvestipsQuotesContext())
            {
                quotes = db.Quotes.Where(
                    q => q.IsBullEight45Degreed && q.Symbol == symbol
                ).ToList();
            }

            if (quotes.Count > 0)
            {
                quotes.Add(quotes.Last());
                var ids = Enumerable.Range(1, quotes.Count()).ToList();
                var times = quotes.Select(x => Convert.ToInt64(ToUnixTimeStamp(x.TimeStampDateTime)))
                    .ToList();
                var colors = quotes.Select(x => "black").ToList();
                var texts = quotes.Select(x => "BullBigEight").ToList();
                var labels = quotes.Select(x => "B8").ToList();
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




        private static bool PreviousTwoAreInRange(int i, Quote quote, List<Quote> quotes, int markCount)
        {
            var timeDaysLimit = 10;
            if (markCount == 3)
            {
                return true;
            }

            if (markCount == 2 && i > 0)
            {
                if ((i - 1) > 0)
                {
                    var previousQuote1 = quotes[i - 1];
                    if (quote.TimeStampDateTime.Subtract(previousQuote1.TimeStampDateTime).Days < timeDaysLimit)
                    {
                        return true;
                    }
                }
            }
            else if (markCount == 1 && i > 1)
            {
                var previousQuote1 = quotes[i - 1];
                var previousQuote2 = quotes[i - 2];
                if (quote.TimeStampDateTime.Subtract(previousQuote1.TimeStampDateTime).Days < timeDaysLimit &&
                    quote.TimeStampDateTime.Subtract(previousQuote2.TimeStampDateTime).Days < timeDaysLimit)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

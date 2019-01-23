using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InvestipsMasterQuotesFunctions.DTO;
using InvestipsMasterQuotesFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using TicTacTec.TA.Library;
using YahooFinanceApi;

namespace InvestipsMasterQuotesFunctions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {

            var history = await Yahoo.GetHistoricalAsync("AAPL", new DateTime(2018,12, 1), DateTime.Now, Period.Daily);

            var mvAvgs10Info = GetMovingAveragesByPeriod(history, 10);
            foreach (var candle in history)
            {
                Console.WriteLine($"DateTime: {candle.DateTime}, Open: {candle.Open}, High: {candle.High}, Low: {candle.Low}, Close: {candle.Close}, Volume: {candle.Volume}, AdjustedClose: {candle.AdjustedClose}");
            }

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

        public static MovingAvgInfo GetMovingAveragesByPeriod(IEnumerable<Candle> historicalQuotes, int period)
        {
            int outBegIndex = 0;
            int outNbElement = 0;
            var closePrices = historicalQuotes.Select(x => Convert.ToSingle(x.Close)).ToArray();
            var outMovingAverages = new double[closePrices.Length];

            var resultState = TicTacTec.TA.Library.Core.MovingAverage(
                0,
                closePrices.Length - 1,
                closePrices, period,
                Core.MAType.Ema,
                out outBegIndex,
                out outNbElement,
                outMovingAverages);

            return new MovingAvgInfo
            {
                StartIndex = outBegIndex,
                EndIndex = outNbElement,
                MovingAverages = outMovingAverages,
                Period = period
            };
        }
    }
}

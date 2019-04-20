using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FinanceQuoteService;
using Investips.Data.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using YahooFinanceApi;

namespace InvestipsMasterQuotesFunctions.Functions
{
    public static class QuotePopulatorFunction
    {
        [FunctionName("QuotePopulatorFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            IList<Quote> quotes = new List<Quote>();
            var quoteMaker = new QuoteMaker(new InvestipsQuoteBuilder());

            var httpClient = GetHttpClient();

            var symbols = await GetStockSymbols(httpClient);

            foreach (var symbol in symbols)
            {
                var history = await Yahoo.GetHistoricalAsync(symbol, new DateTime(2016, 1, 1), DateTime.Now, Period.Daily);
                quoteMaker.BuildQuote(history, symbol);
                quotes = quoteMaker.GetQuotes();
            }

            var quoteService = new QuoteService();
            
            using (var db = new InvestipsQuotesContext())
            {
                foreach (var quote in quotes)
                {
                    db.Quotes.Add(quote);
                    await db.SaveChangesAsync();
                    log.Info("Quote Created");
                }
                //db.Quotes.Add(new Quote { Symbol = "AAPL", Open = 100, High = 103, Low = 99, Close = 102, TimeStampDateTime = DateTime.Now });          
            }

            return req.CreateResponse(HttpStatusCode.OK, "Done ");
        }

        private static HttpClient GetHttpClient()
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://enigmatic-waters-56889.herokuapp.com/");
            _httpClient.Timeout = new TimeSpan(0, 0, 45);
            return _httpClient;
        }

        private static async Task<IEnumerable<string>> GetStockSymbols(HttpClient _httpClient)
        {
            var response = await _httpClient.GetAsync("api/udf/allsymbols");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var symbols = JsonConvert.DeserializeObject<IEnumerable<string>>(content);
            // return symbols;
            return new List<string>() { "AAPL" };
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EntityFramework.Utilities;
using FinanceQuoteService;
using Flurl.Http;
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
            List<Quote> quotes = new List<Quote>();
            var quoteMaker = new QuoteMaker(new InvestipsQuoteBuilder());

            var httpClient = GetHttpClient();

            var symbols = await GetStockSymbols(httpClient);
            var subsetSymbols = symbols.Where(s => s.StartsWith("C") ).ToList();
            var index = 1;
            foreach (var symbol in subsetSymbols)
            {
                //if (index < 3)
                //{
                    try
                    {
                        var history = await Yahoo.GetHistoricalAsync(symbol, new DateTime(2019, 3, 1), DateTime.Now, Period.Daily);
                        quoteMaker.BuildQuote(history, symbol);
                        quotes.AddRange(quoteMaker.GetQuotes());
                        index++;

                        Debug.WriteLine($"------------------------------------------------------------------------------");
                        Debug.WriteLine($"                       Symbol {symbol} - with index {index}                   ");
                        Debug.WriteLine($"______________________________________________________________________________");
                }
                    catch (FlurlHttpException ex)
                    {
                        Debug.WriteLine($"Error fetching  {symbol} quotes ", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error fetching  {symbol} quotes ", ex.Message);
                    }
               // }
            }

            using (var db = new InvestipsQuotesContext())
            {
                var today = DateTime.Today;

                var existingSingleQuote = await db.Quotes.Where(q => q.TimeStampDateTime != null && q.Symbol.StartsWith("C")).OrderByDescending(q => q.TimeStampDateTime).FirstOrDefaultAsync();
                if (existingSingleQuote != null)
                {
                    quotes = quotes.Where(q => q.TimeStampDateTime <= today &&
                                                           q.TimeStampDateTime > existingSingleQuote.TimeStampDateTime).ToList();
                }

            }


            using (var db = new InvestipsQuotesContext())
            {
                EFBatchOperation.For(db, db.Quotes).InsertAll(quotes);
                log.Info("Quote Created");
            } 

            //using (var db = new InvestipsQuotesContext())
            //{
            //    foreach (var quote in quotes)
            //    {
            //        db.Quotes.Add(quote);
            //        await db.SaveChangesAsync();
            //        log.Info("Quote Created");
            //    }
            //    //db.Quotes.Add(new Quote { Symbol = "AAPL", Open = 100, High = 103, Low = 99, Close = 102, TimeStampDateTime = DateTime.Now });          
            //}

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
             return symbols;
            //return new List<string>() { "AAPL" };
        }

    }
}

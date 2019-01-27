using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace FinanceQuoteService
{
    public class QuoteService
    {
        public async Task<IEnumerable<Candle>> GetHistoricalQuotes()
        {
            return await Yahoo.GetHistoricalAsync("AAPL", new DateTime(2018, 12, 1), DateTime.Now, Period.Daily);
            
            //foreach (var candle in history)
            //{
            //    Console.WriteLine($"DateTime: {candle.DateTime}, Open: {candle.Open}, High: {candle.High}, Low: {candle.Low}, Close: {candle.Close}, Volume: {candle.Volume}, AdjustedClose: {candle.AdjustedClose}");
            //}

           // return history.ToList();
        }
    }
}

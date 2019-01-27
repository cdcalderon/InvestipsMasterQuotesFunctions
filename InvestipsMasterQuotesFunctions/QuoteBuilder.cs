using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvestipsMasterQuotesFunctions.Models;
using YahooFinanceApi;

namespace InvestipsMasterQuotesFunctions
{
    public abstract class QuoteBuilder
    {
        protected Quote Quote;
        protected IEnumerable<Candle> QuoteCandles;
        public Quote GetQuote()
        {
            return Quote;
        }

        public void CreateNewQuote(IEnumerable<Candle> quoteCandles)
        {
            Quote = new Quote();
            this.QuoteCandles = quoteCandles;
        }

        public abstract void ApplyCandleBarQuote();
        public abstract void ApplyMovingAverages();
        public abstract void ApplyMacds();
        public abstract void ApplyStochatics();
        public abstract void ApplySignals();

    }
}

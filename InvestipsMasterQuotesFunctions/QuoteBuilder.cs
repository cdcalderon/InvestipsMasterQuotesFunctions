using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investips.Data.Models;
using YahooFinanceApi;

namespace InvestipsMasterQuotesFunctions
{
    public abstract class QuoteBuilder
    {
        protected IList<Quote> Quotes;
        protected IEnumerable<Candle> QuoteCandles;
        public IList<Quote> GetQuotes()
        {
            return Quotes;
        }

        public void CreateNewQuote(IEnumerable<Candle> quoteCandles)
        {
            this.Quotes = new List<Quote>();
            this.QuoteCandles = quoteCandles;
        }

        public abstract void ApplyCandleBarQuote();
        public abstract void ApplyMovingAverages();
        public abstract void ApplyMacds();
        public abstract void ApplyStochatics();
        public abstract void ApplySignals();
        public abstract void ApplyPriceCrossingUpMovingAverages();
        public abstract void ApplyStochasticCrossingUp25();
        public abstract void ApplyMacdCrossingHorizontalUp();

    }
}

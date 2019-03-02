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
        protected string Symbol;
        public IList<Quote> GetQuotes()
        {
            return Quotes;
        }

        public void CreateNewQuote(IEnumerable<Candle> quoteCandles, string symbol)
        {
            this.Quotes = new List<Quote>();
            this.QuoteCandles = quoteCandles;
            this.Symbol = symbol;
        }

        public abstract void ApplyCandleBarQuote();
        public abstract void ApplyMovingAverages();
        public abstract void ApplyMacds();
        public abstract void ApplyStochatics1405();
        public abstract void ApplyStochatics101();
        public abstract void ApplySignals();
        public abstract void ApplyPriceCrossingUpMovingAverage30();
        public abstract void ApplyPriceCrossingUpMovingAverage7();
        public abstract void ApplyStochasticCrossingUp25();
        public abstract void ApplyStochasticCrossingUp20();
        public abstract void ApplyMacdCrossingHorizontalUp();
        public abstract void ApplyIsMovingAvg30PointingUp();
        public abstract void ApplyIsStochMovingUp();
        public abstract void ApplyBull45DegreeCheck();
        public abstract void ApplyNewLowCheck();

    }
}

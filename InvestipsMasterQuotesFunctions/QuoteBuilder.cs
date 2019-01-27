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
        protected Candle QuoteCandle;
        public Quote GetQuote()
        {
            return Quote;
        }

        public void CreateNewQuote(Candle quoteCandle)
        {
            Quote = new Quote();
            this.QuoteCandle = quoteCandle;
        }

        public abstract void ApplyCandleBarQuote();
        public abstract void ApplyMovingAverages();
        public abstract void ApplyMacds();
        public abstract void ApplyStochatics();
        public abstract void ApplySignals();

    }
}

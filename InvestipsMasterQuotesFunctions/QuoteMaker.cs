using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investips.Data.Models;
using YahooFinanceApi;

namespace InvestipsMasterQuotesFunctions
{
    public class QuoteMaker
    {
        private readonly QuoteBuilder builder;

        public QuoteMaker(QuoteBuilder builder)
        {
            this.builder = builder;
        }

        public void BuildQuote(IEnumerable<Candle> candleQuotes)
        {
            builder.CreateNewQuote(candleQuotes);
            builder.ApplyCandleBarQuote();
            builder.ApplyMovingAverages();
            builder.ApplyStochatics();
            builder.ApplyMacds();
            builder.ApplyPriceCrossingUpMovingAverages();
            builder.ApplyStochasticCrossingUp25();
            builder.ApplyMacdCrossingHorizontalUp();

            builder.ApplySignals();
        }

        public IList<Quote> GetQuotes()
        {
            return builder.GetQuotes();
        }
    }
}

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

        public void BuildQuote(IEnumerable<Candle> candleQuotes, string symbol)
        {
            builder.CreateNewQuote(candleQuotes, symbol);
            builder.ApplyCandleBarQuote();
            builder.ApplyMovingAverages();
           // builder.ApplyCandleDoji();
            builder.ApplyStochatics1405();
            builder.ApplyStochatics101();
            builder.ApplyMacds();
            builder.ApplyPriceCrossingUpMovingAverage30();
            builder.ApplyPriceCrossingDownMovingAverage30();
            builder.ApplyPriceCrossingUpMovingAverage7();
            builder.ApplyPriceCrossingDownMovingAverage7();
            builder.ApplyStochasticCrossingUp25();
            builder.ApplyStochasticCrossingDown75();
            builder.ApplyStochasticCrossingUp20();
            builder.ApplyStochasticCrossingDown80();
            builder.ApplyMacdCrossingHorizontalUp();
            builder.ApplyMacdCrossingHorizontalDown();
            builder.ApplyIsMovingAvg30PointingUp();
            builder.ApplyIsMovingAvg30PointingDown();
            builder.ApplyIsStochMovingUp();
            builder.ApplyIsStochMovingDown();
            builder.ApplyNewLowCheck();

            builder.ApplySignals();
        }

        public IList<Quote> GetQuotes()
        {
            return builder.GetQuotes();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvestipsMasterQuotesFunctions.Models;
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

        public void BuildQuote(Candle candleQuote)
        {
            builder.CreateNewQuote(candleQuote);
            builder.ApplyCandleBarQuote();
            builder.ApplyMovingAverages();
            builder.ApplyStochatics();
            builder.ApplyMacds();
            builder.ApplySignals();
        }

        public Quote GetQuote()
        {
            return builder.GetQuote();
        }
    }
}

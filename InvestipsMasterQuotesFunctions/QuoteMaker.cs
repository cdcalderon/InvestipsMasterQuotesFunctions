﻿using System;
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
            builder.ApplyStochatics1405();
            builder.ApplyStochatics101();
            builder.ApplyMacds();
            builder.ApplyPriceCrossingUpMovingAverage30();
            builder.ApplyPriceCrossingUpMovingAverage7();
            builder.ApplyStochasticCrossingUp25();
            builder.ApplyStochasticCrossingUp20();
            builder.ApplyMacdCrossingHorizontalUp();
            builder.ApplyIsMovingAvg30PointingUp();
            builder.ApplyIsStochMovingUp();
            builder.ApplyNewLowCheck();
            builder.ApplyBull45DegreeCheck();
            builder.ApplySuperGapCheck();
            builder.ApplySuperGapBear();


            builder.ApplySignals();
        }

        public IList<Quote> GetQuotes()
        {
            return builder.GetQuotes();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceQuoteService;
using YahooFinanceApi;

namespace InvestipsMasterQuotesFunctions
{
    public class InvestipsQuoteBuilder : QuoteBuilder
    {
        public override void ApplyCandleBarQuote()
        {
            var lastQuote = this.QuoteCandles.First();
            this.Quote.Open = lastQuote.Open;
        }

        public override void ApplyMovingAverages()
        {
            Console.WriteLine("ApplyMovingAverages()");
        }

        public override void ApplyMacds()
        {
            Console.WriteLine("ApplyMacds()");
        }

        public override void ApplyStochatics()
        {
            Console.WriteLine("ApplyStochatics()");
        }

        public override void ApplySignals()
        {
            Console.WriteLine("ApplySignals()");
        }
    }
}

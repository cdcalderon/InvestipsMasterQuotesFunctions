using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceQuoteService;
using InvestipsMasterQuotesFunctions.DTO;
using InvestipsMasterQuotesFunctions.Models;
using TicTacTec.TA.Library;
using YahooFinanceApi;

namespace InvestipsMasterQuotesFunctions
{
    public class InvestipsQuoteBuilder : QuoteBuilder
    {
        public override void ApplyCandleBarQuote()
        {
            foreach (var candle in this.QuoteCandles)
            {
                var quote = new Quote();
                quote.Open = candle.Open;
                quote.High = candle.High;
                quote.Close = candle.Close;
                quote.Low = candle.Low;
                quote.TimeStampDateTime = candle.DateTime;

                this.Quotes.Add(quote);
            }
        }

        public override void ApplyMovingAverages()
        {
            var periods = new[] { 7, 10, 30};
            Console.WriteLine("ApplyMovingAverages()");

            foreach (var period in periods)
            {
                int outBegIndex = 0;
                int outNbElement = 0;
                var closePrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Close)).ToArray();
                var outMovingAverages = new double[closePrices.Length];

                var resultState = TicTacTec.TA.Library.Core.MovingAverage(
                    0,
                    closePrices.Length - 1,
                    closePrices, period,
                    Core.MAType.Ema,
                    out outBegIndex,
                    out outNbElement,
                    outMovingAverages);

                var mvgAvg = new MovingAvgInfo
                {
                    StartIndex = outBegIndex,
                    EndIndex = outNbElement,
                    MovingAverages = outMovingAverages,
                    Period = period
                };

                MergeMvgAvg(mvgAvg);
            }
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


        private void MergeMvgAvg(MovingAvgInfo movingAvgInfo)
        {
            var idx = movingAvgInfo.EndIndex;
            for (int i = 0; i < movingAvgInfo.EndIndex; i++)
            {
                switch (movingAvgInfo.Period)
                {
                    case 7:
                        this.Quotes[movingAvgInfo.StartIndex + i].MovingAvg7 = movingAvgInfo.MovingAverages[i];
                        break;
                    case 10:
                        this.Quotes[movingAvgInfo.StartIndex + i].MovingAvg10 = movingAvgInfo.MovingAverages[i];
                        break;
                    case 30:
                        this.Quotes[movingAvgInfo.StartIndex + i].MovingAvg30 = movingAvgInfo.MovingAverages[i];
                        break;
                }

                idx++;
            }
        }
    }
}

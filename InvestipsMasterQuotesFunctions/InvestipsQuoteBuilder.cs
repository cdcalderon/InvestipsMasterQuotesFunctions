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
            int outBegIndex = 0;
            int outNbElement = 0;
            var closePrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Close)).ToArray();
            var outMacds = new double[closePrices.Length];
            var outMacdSignals = new double[closePrices.Length];
            var outMacdHis = new double[closePrices.Length];

            var resultState = TicTacTec.TA.Library.Core.Macd(0, closePrices.Length - 1, closePrices,
                8, 17, 9, out outBegIndex, out outNbElement, outMacds, outMacdSignals, outMacdHis);

            var macd = new MacdInfo
            {
                StartIndex = outBegIndex,
                EndIndex = outNbElement,
                Macds = outMacdHis
            };

            MergeMacd(macd);
        }

        public override void ApplyStochatics()
        {
            int outBegIndex = 0;
            int outNbElement = 0;
            var closePrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Close)).ToArray();
            var highPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.High)).ToArray();
            var lowPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Low)).ToArray();
            var slowKs = new double[closePrices.Length];
            var slowDs = new double[closePrices.Length];

            var resultState = TicTacTec.TA.Library.Core.Stoch(0, closePrices.Length - 1, highPrices,
                lowPrices, closePrices, 14, 5, 0, 5, Core.MAType.Ema, out outBegIndex, out outNbElement, slowKs, slowDs);

            var stochastics =  new StochasticsInfo
            {
                StartIndex = outBegIndex,
                EndIndex = outNbElement,
                StochasticsSlowsK = slowKs
            };

            MergeStochastics(stochastics);
        }

        public override void ApplySignals()
        {
            ApplyBullStoch307Signal();
            ApplyBearStoch307Signal();
            ApplyBullThreeArrowSignal();
            ApplyBearThreeArrowSignal();
            ApplyBullBigEightSignal();
            ApplyBearBigEightSignal();
            ApplySuperGapSignal();
        }

        private void ApplyBullStoch307Signal()
        {
            
        }

        private void ApplyBearStoch307Signal()
        {

        }

        private void ApplyBullThreeArrowSignal()
        {

        }

        private void ApplyBearThreeArrowSignal()
        {

        }

        private void ApplySuperGapSignal()
        {

        }

        private void ApplyBearBigEightSignal()
        {

        }

        private void ApplyBullBigEightSignal()
        {

        }

        private void MergeMvgAvg(MovingAvgInfo movingAvgInfo)
        {
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
            }
        }

        private void MergeMacd(MacdInfo macdInfo)
        {
            for (int i = 0; i < macdInfo.EndIndex; i++)
            {
                this.Quotes[macdInfo.StartIndex + i].Macd8179 = macdInfo.Macds[i];
            }
        }

        private void MergeStochastics(StochasticsInfo stochasticsInfo)
        {
            for (int i = 0; i < stochasticsInfo.EndIndex; i++)
            {
                this.Quotes[stochasticsInfo.StartIndex + i].Stochastics14505 = stochasticsInfo.StochasticsSlowsK[i];
            }
        }
    }
}

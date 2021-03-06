﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceQuoteService;
using Investips.Data.Models;
using InvestipsMasterQuotesFunctions.DTO;
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
                quote.Symbol = this.Symbol;
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
                var closePrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Close)).ToArray();
                var outMovingAverages = new double[closePrices.Length];

                var resultState = TicTacTec.TA.Library.Core.MovingAverage(
                    0,
                    closePrices.Length - 1,
                    closePrices, 
                    period,
                    Core.MAType.Ema,
                    out var outBegIndex,
                    out var outNbElement,
                    outMovingAverages);

                var mvgAvg = new MovingAvgInfo
                {
                    StartIndex = outBegIndex,
                    EndIndex = outNbElement,
                    MovingAverages = outMovingAverages.Select(d => Convert.ToDecimal(d)).ToArray(),
                    Period = period
                };

                MergeMvgAvg(mvgAvg);
            }
        }

        public override void ApplyCandleDoji()
        {
            var closePrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Close)).ToArray();
            var highPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.High)).ToArray();
            var lowPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Low)).ToArray();
            var openPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Open)).ToArray();
            var outDojis = new int[closePrices.Length];

            //var tt = TicTacTec.TA.Library.Core.CdlEngulfing(
            //    0,
            //    closePrices.Length - 1,
            //    openPrices,
            //    highPrices,
            //    lowPrices,
            //    closePrices,
            //    out var outBegIndex,
            //    out var outNbElement,
            //    outDojis);

            //var tt = TicTacTec.TA.Library.Core.CdlDarkCloudCover(
            //    0,
            //    closePrices.Length - 1,
            //    openPrices,
            //    highPrices,
            //    lowPrices,
            //    closePrices,
            //    .50,
            //    out var outBegIndex,
            //    out var outNbElement,
            //    outDojis);

            var tt = TicTacTec.TA.Library.Core.CdlEveningStar(
                0,
                closePrices.Length - 1,
                openPrices,
                highPrices,
                lowPrices,
                closePrices,
                .50,
                out var outBegIndex,
                out var outNbElement,
                outDojis);

            for (int i = 0; i < outNbElement; i++)
            {
                this.Quotes[outBegIndex + i].IsDoji = outDojis[i] > 0 || outDojis[i] < 0 ? true: false;
            }

            
        }
        
        public override void ApplyMacds()
        {
            var closePrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Close)).ToArray();
            var outMacds = new double[closePrices.Length];
            var outMacdSignals = new double[closePrices.Length];
            var outMacdHis = new double[closePrices.Length];

            TicTacTec.TA.Library.Core.Macd(0, closePrices.Length - 1, closePrices,
                8, 17, 9, out var outBegIndex, out var outNbElement, outMacds, outMacdSignals, outMacdHis);

            var macd = new MacdInfo
            {
                StartIndex = outBegIndex,
                EndIndex = outNbElement,
                Macds = outMacdHis.Select(d => Convert.ToDecimal(d)).ToArray()
            };

            MergeMacd(macd);
        }

        public override void ApplyStochatics1405()
        {
            var closePrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Close)).ToArray();
            var highPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.High)).ToArray();
            var lowPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Low)).ToArray();
            var slowKs = new double[closePrices.Length];
            var slowDs = new double[closePrices.Length];

            TicTacTec.TA.Library.Core.Stoch(0, closePrices.Length - 1, highPrices,
                lowPrices, closePrices, 14, 5, 0, 5, Core.MAType.Ema, out var outBegIndex, out var outNbElement, slowKs, slowDs);

            var stochastics =  new StochasticsInfo
            {
                StartIndex = outBegIndex,
                EndIndex = outNbElement,
                StochasticsSlowsK = slowKs.Select(d => Convert.ToDecimal(d)).ToArray()
            };

            MergeStochastics(stochastics, 14);
        }

        public override void ApplyStochatics101()
        {
            var closePrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Close)).ToArray();
            var highPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.High)).ToArray();
            var lowPrices = this.QuoteCandles.Select(x => Convert.ToSingle(x.Low)).ToArray();
            var slowKs = new double[closePrices.Length];
            var slowDs = new double[closePrices.Length];

            TicTacTec.TA.Library.Core.Stoch(0, closePrices.Length - 1, highPrices,
                lowPrices, closePrices, 10, 1, 0, 1, Core.MAType.Ema, out var outBegIndex, out var outNbElement, slowKs, slowDs);

            var stochastics = new StochasticsInfo
            {
                StartIndex = outBegIndex,
                EndIndex = outNbElement,
                StochasticsSlowsK = slowKs.Select(d => Convert.ToDecimal(d)).ToArray()
            };

            MergeStochastics(stochastics, 10);
        }

        public override void ApplySignals()
        {
            ApplyBullStoch307Signal();
            ApplyBearStoch307Signal();

            ApplyBullThreeArrowSignal();
            ApplyBearThreeArrowSignal();

            ApplyBull45DegreeSignal();
            ApplyBear45DegreeSignal();

            ApplySuperGapBearSignal();
            ApplySuperGapSignal();
        }

        public override void ApplyPriceCrossingUpMovingAverage30()
        {
            // define a small tolerance on our checks to avoid bouncing
            const decimal tolerance = 0.00015m;
            for (var i = 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousClose = previousQuote.Close;
                var previousMovAvg30 = previousQuote.MovingAvg30;
                var currentClose = currentQuote.Close;
                var currentMovingAvg = currentQuote.MovingAvg30;

                if (currentClose > currentMovingAvg && previousClose < previousMovAvg30)
                {
                    this.Quotes[i].IsPriceCrossMovAvg30Up = true;
                }
            }
        }

        public override void ApplyPriceCrossingDownMovingAverage30()
        {
            // define a small tolerance on our checks to avoid bouncing
            const decimal tolerance = 0.00015m;
            for (var i = 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousClose = previousQuote.Close;
                var previousMovAvg30 = previousQuote.MovingAvg30;
                var currentClose = currentQuote.Close;
                var currentMovingAvg = currentQuote.MovingAvg30;

                if (currentClose < currentMovingAvg && previousClose > previousMovAvg30)
                {
                    this.Quotes[i].IsPriceCrossMovAvg30Down = true;
                }
            }
        }

        public override void ApplyIsMovingAvg30PointingUp()
        {
            const decimal tolerance = 0.00015m;
            for (var i = 30 + 3; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 3];
                var currentQuote = this.Quotes[i];
                var currentClose = currentQuote.Close;
                var currentMovingAvg = currentQuote.MovingAvg30;
                var previousClose = previousQuote.Close;
                var previousMovAvg30 = previousQuote.MovingAvg30;
                
                if (currentMovingAvg > previousMovAvg30)
                {
                   
                    this.Quotes[i].IsMovingAvg30PointingUp = true;
                    Debug.WriteLine($"ApplyIsMovingAvg30PointingUp {currentQuote.TimeStampDateTime.ToShortDateString()}");
                }
            }
        }

        public override void ApplyIsMovingAvg30PointingDown()
        {
            const decimal tolerance = 0.00015m;
            for (var i = 30 + 3; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 3];
                var currentQuote = this.Quotes[i];
                var currentClose = currentQuote.Close;
                var currentMovingAvg = currentQuote.MovingAvg30;
                var previousClose = previousQuote.Close;
                var previousMovAvg30 = previousQuote.MovingAvg30;

                if (currentMovingAvg < previousMovAvg30)
                {

                    this.Quotes[i].IsMovingAvg30PointingDown = true;
                    Debug.WriteLine($"ApplyIsMovingAvg30PointingDown {currentQuote.TimeStampDateTime.ToShortDateString()}");
                }
            }
        }

        public override void ApplyIsStochMovingUp()
        {
            const decimal tolerance = 0.00015m;
            for (var i = 30 ; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var currentStoch14505 = currentQuote.Stochastics14505;
                var previousStoch14505 = previousQuote.Stochastics14505;

                if (currentStoch14505 > previousStoch14505)
                {

                    this.Quotes[i].IsStoch14505PointingUp = true;
                    Debug.WriteLine($"ApplyIsStoch14505PointingUp {currentQuote.TimeStampDateTime.ToShortDateString()}");
                }
            }
        }

        public override void ApplyIsStochMovingDown()
        {
            const decimal tolerance = 0.00015m;
            for (var i = 30; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var currentStoch14505 = currentQuote.Stochastics14505;
                var previousStoch14505 = previousQuote.Stochastics14505;

                if (currentStoch14505 < previousStoch14505)
                {

                    this.Quotes[i].IsStoch14505PointingDown = true;
                    Debug.WriteLine($"ApplyIsStoch14505PointingDown {currentQuote.TimeStampDateTime.ToShortDateString()}");
                }
            }
        }

        public override void ApplyPriceCrossingUpMovingAverage7()
        {
            // define a small tolerance on our checks to avoid bouncing
            const decimal tolerance = 0.00015m;
            for (var i = 7 + 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousClose = previousQuote.Close;
                var previousMovAvg7 = previousQuote.MovingAvg7;
                var currentClose = currentQuote.Close;
                var currentMovingAvg7 = currentQuote.MovingAvg7;

                if (currentClose > currentMovingAvg7 && previousClose < previousMovAvg7)
                {
                    this.Quotes[i].IsPriceCrossMovAvg7Up = true;
                }
            }
        }

        public override void ApplyPriceCrossingDownMovingAverage7()
        {
            // define a small tolerance on our checks to avoid bouncing
            const decimal tolerance = 0.00015m;
            for (var i = 7 + 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousClose = previousQuote.Close;
                var previousMovAvg7 = previousQuote.MovingAvg7;
                var currentClose = currentQuote.Close;
                var currentMovingAvg7 = currentQuote.MovingAvg7;

                if (currentClose < currentMovingAvg7 && previousClose > previousMovAvg7)
                {
                    this.Quotes[i].IsPriceCrossMovAvg7Down = true;
                }
            }
        }

        public override void ApplyStochasticCrossingUp25()
        {
            for (var i = 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousStoch14505 = previousQuote.Stochastics14505;
                var currentStoch14505 = currentQuote.Stochastics14505;

                if (currentStoch14505 > 25 && previousStoch14505 < 25)
                {
                    this.Quotes[i].IsStoch145Cossing25Up = true;
                }
            }
        }

        public override void ApplyStochasticCrossingDown75()
        {
            for (var i = 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousStoch14505 = previousQuote.Stochastics14505;
                var currentStoch14505 = currentQuote.Stochastics14505;

                if (currentStoch14505 < 75 && previousStoch14505 > 75)
                {
                    this.Quotes[i].IsStoch145Cossing75Down = true;
                }
            }
        }

        public override void ApplyStochasticCrossingUp20()
        {
            for (var i = 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousStoch101 = previousQuote.Stochastics101;
                var currentStoch101 = currentQuote.Stochastics101;

                if (currentStoch101 > 20 && previousStoch101 < 20)
                {
                    this.Quotes[i].IsStoch101Cossing20Up = true;
                    Debug.WriteLine($"ApplyStochasticCrossingUp20 {currentQuote.TimeStampDateTime.ToShortDateString()}");
                }
            }
        }

        public override void ApplyStochasticCrossingDown80()
        {
            for (var i = 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousStoch101 = previousQuote.Stochastics101;
                var currentStoch101 = currentQuote.Stochastics101;

                if (currentStoch101 < 80 && previousStoch101 > 80)
                {
                    this.Quotes[i].IsStoch101Cossing80Down = true;
                    Debug.WriteLine($"ApplyStochasticCrossingDown80 {currentQuote.TimeStampDateTime.ToShortDateString()}");
                }
            }
        }

        public override void ApplyMacdCrossingHorizontalUp()
        {
            for (var i = 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousMacd8179 = previousQuote.Macd8179;
                var currentMacd8179 = currentQuote.Macd8179;

                if (currentMacd8179 > 0 && previousMacd8179 <= 0)
                {
                    this.Quotes[i].IsMacdCrossingHorizontalUp = true;
                }
            }
        }

        public override void ApplyMacdCrossingHorizontalDown()
        {
            for (var i = 1; i < this.Quotes.Count; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];
                var previousMacd8179 = previousQuote.Macd8179;
                var currentMacd8179 = currentQuote.Macd8179;

                if (currentMacd8179 < 0 && previousMacd8179 >= 0)
                {
                    this.Quotes[i].IsMacdCrossingHorizontalDown = true;
                }
            }
        }

        public override void ApplyNewLowCheck()
        {
            for (var i = 1; i < this.Quotes.Count - 1; i++)
            {
                var previousQuoteLow = this.Quotes[i - 1].Low;
                var currentQuoteLow = this.Quotes[i].Low;
                var nextQuoteLow = this.Quotes[i + 1].Low;

                if (currentQuoteLow <= previousQuoteLow && currentQuoteLow <= nextQuoteLow)
                {
                    this.Quotes[i].IsNewLow = true;
                }
            }
        }

        //public override void ApplyBull45DegreeCheck()
        //{
        //    var incrementCheckIndex = 1;
        //    decimal initialQuotePrice = 0;

        //    foreach (var currentQuote in this.Quotes)
        //    {
        //        var currentLow = currentQuote.Low;

        //        decimal validIncrementPrice = 0;
        //        if (currentQuote.IsNewLow)
        //        {
        //            initialQuotePrice = currentLow;

        //            incrementCheckIndex = incrementCheckIndex + 1;
        //            continue;
        //        }

        //        if (incrementCheckIndex < 2) continue;

        //        validIncrementPrice = initialQuotePrice + (initialQuotePrice * 0.0035m) * (incrementCheckIndex - 1);

        //        if (currentLow >= validIncrementPrice)
        //        {
        //            incrementCheckIndex = incrementCheckIndex + 1;
        //            currentQuote.FourtyFiveDegreeLevel = incrementCheckIndex;
        //            if (incrementCheckIndex == 8)
        //            {
        //                currentQuote.IsBullEight45Degreed = true;
        //            }
        //        }
        //        else
        //        {
        //            incrementCheckIndex = 1;
        //        }
        //    }
        //}

        //public override void ApplyBull45DegreeCheck()
        //{
        //    var incrementCheckIndex = 1;
        //    decimal initialQuotePrice = 0;


        //    foreach (var currentQuote in this.Quotes)
        //    {
        //        if (currentQuote.Symbol == "ANDE" && currentQuote.TimeStampDateTime.Day == 17 &&
        //                                  currentQuote.TimeStampDateTime.Year == 2016 &&
        //                                  currentQuote.TimeStampDateTime.Month == 5)
        //        {
        //            var t = string.Empty;
        //        }
        //        var currentLow = currentQuote.Low;

        //        decimal validIncrementPrice = 0;
        //        if (currentQuote.IsNewLow)
        //        {
        //            initialQuotePrice = currentLow;

        //            incrementCheckIndex = incrementCheckIndex + 1;
        //            continue;
        //        }

        //        //if (incrementCheckIndex < 2) continue;

        //        validIncrementPrice = initialQuotePrice + (initialQuotePrice * 0.0040m) * (incrementCheckIndex - 1);

        //        if (currentLow >= validIncrementPrice)
        //        {
        //            incrementCheckIndex = incrementCheckIndex + 1;
        //            currentQuote.FourtyFiveDegreeLevel = incrementCheckIndex;
        //            if (incrementCheckIndex == 9)
        //            {
        //                currentQuote.IsBullEight45Degreed = true;
        //            }
        //        }
        //        else
        //        {
        //            incrementCheckIndex = 1;
        //        }
        //    }
        //}

        private void ApplyBull45DegreeSignal()
        {
            var incrementCheckIndex = 1;
            decimal initialQuotePrice = 0;


            foreach (var currentQuote in this.Quotes)
            {
                if (currentQuote.Symbol == "ANDE" && currentQuote.TimeStampDateTime.Day == 11 &&
                    currentQuote.TimeStampDateTime.Year == 2018 &&
                    currentQuote.TimeStampDateTime.Month == 4)
                {
                    var t = string.Empty;
                }
                var currentLow = currentQuote.Low;

                decimal validIncrementPrice = 0;
                validIncrementPrice = initialQuotePrice + (initialQuotePrice * 0.0088m) * (incrementCheckIndex - 1);
                if (currentQuote.IsNewLow)
                {
                    //validIncrementPrice = initialQuotePrice + (initialQuotePrice * 0.0040m) * (incrementCheckIndex - 1);

                    if (currentLow >= validIncrementPrice)
                    {
                        incrementCheckIndex = incrementCheckIndex + 1;
                        currentQuote.FourtyFiveDegreeLevel = incrementCheckIndex;
                        if (incrementCheckIndex == 9)
                        {
                            currentQuote.IsBullEight45Degreed = true;
                            initialQuotePrice = currentLow;
                            incrementCheckIndex = 1;
                        }
                    }
                    else
                    {
                        incrementCheckIndex = 1;
                        initialQuotePrice = currentLow;
                    }

                    
                    continue;
                }

                //if (incrementCheckIndex < 2) continue;

                //validIncrementPrice = initialQuotePrice + (initialQuotePrice * 0.0040m) * (incrementCheckIndex - 1);

                if (currentLow >= validIncrementPrice)
                {
                    incrementCheckIndex = incrementCheckIndex + 1;
                    currentQuote.FourtyFiveDegreeLevel = incrementCheckIndex;
                    if (incrementCheckIndex == 9)
                    {
                        currentQuote.IsBullEight45Degreed = true;
                        initialQuotePrice = currentLow;
                        incrementCheckIndex = 1;
                    }
                }
                else
                {
                    incrementCheckIndex = 1;
                    initialQuotePrice = currentLow;
                }
            }
        }

        public void ApplyBear45DegreeSignal()
        {
            var incrementCheckIndex = 1;
            decimal initialQuotePrice = 0;


            foreach (var currentQuote in this.Quotes)
            {
                if (currentQuote.Symbol == "ANDE" && currentQuote.TimeStampDateTime.Day == 11 &&
                    currentQuote.TimeStampDateTime.Year == 2018 &&
                    currentQuote.TimeStampDateTime.Month == 4)
                {
                    var t = string.Empty;
                }
                var currentLow = currentQuote.Low;

                decimal validIncrementPrice = 0;
                validIncrementPrice = initialQuotePrice + (initialQuotePrice * 0.0088m) * (incrementCheckIndex - 1);
                if (currentQuote.IsNewLow)
                {
                    //validIncrementPrice = initialQuotePrice + (initialQuotePrice * 0.0040m) * (incrementCheckIndex - 1);

                    if (currentLow <= validIncrementPrice)
                    {
                        incrementCheckIndex = incrementCheckIndex + 1;
                        currentQuote.FourtyFiveDegreeLevel = incrementCheckIndex;
                        if (incrementCheckIndex == 9)
                        {
                            currentQuote.IsBearEight45Degreed = true;
                            initialQuotePrice = currentLow;
                            incrementCheckIndex = 1;
                        }
                    }
                    else
                    {
                        incrementCheckIndex = 1;
                        initialQuotePrice = currentLow;
                    }


                    continue;
                }

                //if (incrementCheckIndex < 2) continue;

                //validIncrementPrice = initialQuotePrice + (initialQuotePrice * 0.0040m) * (incrementCheckIndex - 1);

                if (currentLow <= validIncrementPrice)
                {
                    incrementCheckIndex = incrementCheckIndex + 1;
                    currentQuote.FourtyFiveDegreeLevel = incrementCheckIndex;
                    if (incrementCheckIndex == 9)
                    {
                        currentQuote.IsBearEight45Degreed = true;
                        initialQuotePrice = currentLow;
                        incrementCheckIndex = 1;
                    }
                }
                else
                {
                    incrementCheckIndex = 1;
                    initialQuotePrice = currentLow;
                }
            }
        }

        //public override void ApplyBull45DegreeCheck()
        //{
        //    var initialLow = this.Quotes.First().Low;
        //    var incrementCheckIndex = 0;
        //    for (int i = 1; i < this.Quotes.Count; i++)
        //    {
        //        var currentLow = this.Quotes[i].Low;
        //        var validIncrementPrice = initialLow + (initialLow * 0.0040m) * 1;

        //        if (this.Quotes[i].IsNewLow)
        //        {
        //            if (currentLow >= validIncrementPrice)
        //            {
        //                incrementCheckIndex = incrementCheckIndex + 1;
        //                this.Quotes[i].FourtyFiveDegreeLevel = incrementCheckIndex;

        //            }
        //            else
        //            {
        //                incrementCheckIndex = 0;
        //                initialLow = this.Quotes[i].Low;
        //            }
        //            continue;
        //        }

        //        // Need to use the new Low value
        //        if (currentLow >= validIncrementPrice)
        //        {
        //            incrementCheckIndex = incrementCheckIndex + 1;
        //            this.Quotes[i].FourtyFiveDegreeLevel = incrementCheckIndex;
        //            if (incrementCheckIndex == 9)
        //            {
        //                this.Quotes[i].IsBullEight45Degreed = true;
        //                incrementCheckIndex = 0;
        //            }
        //        }
        //        else
        //        {
        //            incrementCheckIndex = 0;
        //            initialLow = this.Quotes[i].Low;
        //        }
        //    }

        //}

        //public override void ApplyBull45DegreeCheck()
        //{
        //    var initialLow = this.Quotes.First().Low;
        //    var incrementCheckIndex = 0;
        //    for (int i = 1; i < this.Quotes.Count; i++)
        //    {
        //        var currentLow = this.Quotes[i].Low;
        //        var validIncrementPrice = initialLow + (initialLow * 0.0040m) * 1;

        //        //if (this.Quotes[i].IsNewLow)
        //        //{
        //        //    if (currentLow >= validIncrementPrice)
        //        //    {
        //        //        incrementCheckIndex = incrementCheckIndex + 1;
        //        //        this.Quotes[i].FourtyFiveDegreeLevel = incrementCheckIndex;

        //        //    }
        //        //    else
        //        //    {
        //        //        incrementCheckIndex = 0;
        //        //        initialLow = this.Quotes[i].Low;
        //        //    }
        //        //    continue;
        //        //}

        //        // Need to use the new Low value
        //        if (currentLow >= validIncrementPrice)
        //        {
        //            incrementCheckIndex = incrementCheckIndex + 1;
        //            this.Quotes[i].FourtyFiveDegreeLevel = incrementCheckIndex;
        //            if (incrementCheckIndex == 9)
        //            {
        //                this.Quotes[i].IsBullEight45Degreed = true;
        //                incrementCheckIndex = 0;
        //            }
        //        }
        //        else
        //        {
        //            incrementCheckIndex = 0;
        //            initialLow = this.Quotes[i].Low;
        //        }
        //    }

        //}


        public void ApplySuperGapSignal()
        {
            var tenPercent = .10m;
            for (var i = 1; i < this.Quotes.Count - 1; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];

                var diffCriteriaAmount = GetDiffAmount(currentQuote.Close);
                var diffCriteriaPercent = currentQuote.Close * tenPercent;

                var diffBetweenCurrentPreviousClose = currentQuote.Close - previousQuote.Close;
                var diffBetweenCurrentLowPreviousHigh = currentQuote.Low - previousQuote.High;

                if (diffBetweenCurrentPreviousClose > diffCriteriaAmount || diffBetweenCurrentPreviousClose >= diffCriteriaPercent)
                {
                    if (diffBetweenCurrentLowPreviousHigh > diffCriteriaAmount || diffBetweenCurrentLowPreviousHigh >= diffCriteriaPercent)
                    {
                        this.Quotes[i].IsSuperGap = true;
                    }
                }
                //return ((diffBetweenCurrentPreviousClose > diffCriteriaAmount) ||
                //        (diffBetweenCurrentPreviousClose >= diffCriteriaPercent)) &&

                //       ((diffBetweenCurrentOpenPreviousClose > diffCriteriaAmount) ||
                //        (diffBetweenCurrentOpenPreviousClose >= diffCriteriaPercent));

            }
        }

        public void ApplySuperGapBearSignal()
        {
            var tenPercent = .10m;
            for (var i = 1; i < this.Quotes.Count - 1; i++)
            {
                var previousQuote = this.Quotes[i - 1];
                var currentQuote = this.Quotes[i];

                var diffCriteriaAmount = GetDiffAmount(currentQuote.Close);
                var diffCriteriaPercent = currentQuote.Close * tenPercent;

                var diffBetweenCurrentPreviousClose = previousQuote.Close - currentQuote.Close;
                var diffBetweenCurrentHighPreviousLow = previousQuote.Low - currentQuote.High;

                if (diffBetweenCurrentPreviousClose > diffCriteriaAmount || diffBetweenCurrentPreviousClose >= diffCriteriaPercent)
                {
                    if (diffBetweenCurrentHighPreviousLow > diffCriteriaAmount || diffBetweenCurrentHighPreviousLow >= diffCriteriaPercent)
                    {
                        this.Quotes[i].IsSuperGapBear = true;
                    }
                }

            }
        }

        public void ApplyBullStoch307Signal()
        {
            foreach (var quote in this.Quotes)
            {
                if (quote.IsPriceCrossMovAvg7Up &&
                    quote.IsStoch101Cossing20Up &&
                    quote.IsMovingAvg30PointingUp
                )
                {
                    Debug.WriteLine($"ApplyBullStoch307Signal {quote.TimeStampDateTime.ToShortDateString()}");
                    quote.IsBullStoch307 = true;
                }
            }
        }

        public void ApplyBearStoch307Signal()
        {
            foreach (var quote in this.Quotes)
            {
                if (quote.IsPriceCrossMovAvg7Down &&
                    quote.IsStoch101Cossing80Down &&
                    quote.IsMovingAvg30PointingDown
                )
                {
                    Debug.WriteLine($"ApplyBearStoch307Signal {quote.TimeStampDateTime.ToShortDateString()}");
                    quote.IsBearStoch307 = true;
                }
            }
        }

        public void ApplyBullThreeArrowSignal()
        {
            var movAvg30Check = false;
            var macdCheck = false;
            var stochasticsCheck = false;

            foreach (var quote in this.Quotes)
            {
                movAvg30Check = quote.IsPriceCrossMovAvg30Up;
                macdCheck = quote.IsMacdCrossingHorizontalUp;
                stochasticsCheck = quote.IsStoch145Cossing25Up;

                if (movAvg30Check && macdCheck && stochasticsCheck && 
                    (quote.Close > quote.MovingAvg30) &&
                    (quote.Stochastics14505 > 25) &&
                    (quote.Macd8179 > 0)
                    )
                {
                    Debug.WriteLine($"{quote.TimeStampDateTime.ToShortDateString()}");
                    quote.IsBullThreeArrow = true;
                    movAvg30Check = false;
                    macdCheck = false;
                    stochasticsCheck = false;
                }
            }
        }

        private void ApplyBearThreeArrowSignal()
        {

            var movAvg30Check = false;
            var macdCheck = false;
            var stochasticsCheck = false;

            foreach (var quote in this.Quotes)
            {
                movAvg30Check = quote.IsPriceCrossMovAvg30Down;
                macdCheck = quote.IsMacdCrossingHorizontalDown;
                stochasticsCheck = quote.IsStoch145Cossing75Down;

                if (movAvg30Check && macdCheck && stochasticsCheck &&
                    (quote.Close < quote.MovingAvg30) &&
                    (quote.Stochastics14505 < 75) &&
                    (quote.Macd8179 < 0)
                )
                {
                    Debug.WriteLine($"{quote.TimeStampDateTime.ToShortDateString()}");
                    quote.IsBearThreeArrow = true;
                    movAvg30Check = false;
                    macdCheck = false;
                    stochasticsCheck = false;
                }
            }
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

        private void MergeStochastics(StochasticsInfo stochasticsInfo, int period)
        {
            for (int i = 0; i < stochasticsInfo.EndIndex; i++)
            {
                switch (period)
                {
                    case 14:
                        this.Quotes[stochasticsInfo.StartIndex + i].Stochastics14505 = stochasticsInfo.StochasticsSlowsK[i];
                        Debug.WriteLine($"ApplyIsStochastics14 {stochasticsInfo.StochasticsSlowsK[i]} -- {this.Quotes[stochasticsInfo.StartIndex + i].TimeStampDateTime}");
                        break;
                    case 10:
                        this.Quotes[stochasticsInfo.StartIndex + i].Stochastics101 = stochasticsInfo.StochasticsSlowsK[i];
                        Debug.WriteLine($"ApplyIsStochastics10 {stochasticsInfo.StochasticsSlowsK[i]} -- {this.Quotes[stochasticsInfo.StartIndex + i].TimeStampDateTime}");
                        break;
                }
                
            }
        }

        private decimal GetDiffAmount(decimal closePrice)
        {
            if (closePrice > 0 && closePrice < 200)
            {
                return 3.5m;
            }
            else if (closePrice > 400 && closePrice < 600)
            {
                return 5.5m;
            }
            else if (closePrice > 600 && closePrice < 800)
            {
                return 7;
            }
            else if (closePrice > 800 && closePrice < 1000)
            {
                return 8;
            }
            else if (closePrice > 1000)
            {
                return 10;
            }

            return 3.5m;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Investips.Data.Models
{
    [Table("Quote", Schema = "Stock")]
    public class Quote
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public decimal High { get; set; }
        [Required]
        public decimal Low { get; set; }
        [Required]
        public decimal Close { get; set; }
        [Required]
        public decimal Open { get; set; }

        public DateTime TimeStampDateTime { get; set; }

        public decimal? MovingAvg7 { get; set; }
        public decimal? MovingAvg10 { get; set; }
        public decimal? MovingAvg30 { get; set; }
        public decimal? Macd8179 { get; set; }
        public decimal? Stochastics14505 { get; set; }
        public decimal? Stochastics101 { get; set; }
        public bool IsPriceCrossMovAvg30Up { get; set; }
        public bool IsPriceCrossMovAvg30Down { get; set; }
        public bool IsPriceCrossMovAvg7Up { get; set; }
        public bool IsPriceCrossMovAvg7Down { get; set; }
        public bool IsStoch145Cossing25Up { get; set; }
        public bool IsStoch145Cossing75Down { get; set; }
        public bool IsStoch101Cossing20Up { get; set; }
        public bool IsStoch101Cossing80Down { get; set; }
        public bool IsMacdCrossingHorizontalUp { get; set; }
        public bool IsMacdCrossingHorizontalDown { get; set; }
        public bool IsMovingAvg30PointingUp { get; set; }
        public bool IsMovingAvg30PointingDown { get; set; }
        public bool IsStoch14505PointingUp { get; set; }
        public bool IsStoch14505PointingDown { get; set; }
        public bool IsNewLow { get; set; }
        public bool IsBullEight45Degreed { get; set; }
        public bool IsBearEight45Degreed { get; set; }
        public int FourtyFiveDegreeLevel { get; set; }

        public bool IsBullThreeArrow { get; set; }
        public bool IsBearThreeArrow { get; set; }
        public bool IsBullStoch307 { get; set; }
        public bool IsBearStoch307 { get; set; }
        public bool IsSuperGap { get; set; }
        public bool IsSuperGapBear { get; set; }


        public bool IsDoji { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestipsMasterQuotesFunctions.Models
{
    [Table("Quote", Schema = "Stock")]
    public class Quote
    {
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

        public decimal MovingAvg7 { get; set; }
        public decimal MovingAvg10 { get; set; }
        public decimal MovingAvg30 { get; set; }
        public decimal Macd8179 { get; set; }
        public decimal Stochastics14505 { get; set; }
    }
}

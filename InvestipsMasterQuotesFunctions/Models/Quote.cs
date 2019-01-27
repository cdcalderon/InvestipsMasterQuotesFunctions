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

        public double MovingAvg7 { get; set; }
        public double MovingAvg10 { get; set; }
        public double MovingAvg30 { get; set; }
    }
}

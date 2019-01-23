using System;
using System.Collections.Generic;
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
        public string Symbol { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Open { get; set; }
    }
}

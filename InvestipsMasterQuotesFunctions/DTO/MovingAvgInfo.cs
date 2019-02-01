using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestipsMasterQuotesFunctions.DTO
{
    public class MovingAvgInfo
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int Period { get; set; }
        public decimal[] MovingAverages { get; set; }
    }
}

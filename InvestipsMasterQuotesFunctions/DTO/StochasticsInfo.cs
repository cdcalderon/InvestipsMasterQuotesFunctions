using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestipsMasterQuotesFunctions.DTO
{
    public class StochasticsInfo
    {
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public double[] StochasticsSlowsK { get; set; }
    }
}

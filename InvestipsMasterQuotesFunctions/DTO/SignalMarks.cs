using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestipsMasterQuotesFunctions.DTO
{
    public class SignalMarks
    {
        public List<int> id { get; set; }
        public List<long> time { get; set; }
        public List<string> color { get; set; }
        public List<string> text { get; set; }
        public List<string> label { get; set; }
        public List<string> labelFontColor { get; set; }
        public List<int> minSize { get; set; }

    }
}

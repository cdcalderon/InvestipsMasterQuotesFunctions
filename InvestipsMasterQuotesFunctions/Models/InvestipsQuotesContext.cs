using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestipsMasterQuotesFunctions.Models
{
    public class InvestipsQuotesContext : DbContext
    {
        public virtual DbSet<Quote> Quotes { get; set; }
    }
}

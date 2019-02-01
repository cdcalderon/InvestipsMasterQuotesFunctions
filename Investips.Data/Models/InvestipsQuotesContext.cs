using System.Data.Entity;

namespace Investips.Data.Models
{
    public class InvestipsQuotesContext : DbContext
    {
        public virtual DbSet<Quote> Quotes { get; set; }
    }
}

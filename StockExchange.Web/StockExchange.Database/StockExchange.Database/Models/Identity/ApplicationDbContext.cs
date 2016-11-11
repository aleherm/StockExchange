using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace StockExchange.Models
{
    // Main Entity DBContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<OwnedStock> OwnedStocks { get; set; }
        public virtual IDbSet<SiteOwnedStocks> SiteOwnedStocks { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
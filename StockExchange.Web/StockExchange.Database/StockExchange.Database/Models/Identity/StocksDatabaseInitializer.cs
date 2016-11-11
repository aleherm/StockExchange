using System.Data.Entity;

namespace StockExchange.Models
{
    public class StocksDatabaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            var siteInitialStocks = InitialData.Create(400, 500, 300, 100, 20, 350, StockExResr.Site_Owned_Id);

            context.SiteOwnedStocks.Add(siteInitialStocks);
        }
    }
}
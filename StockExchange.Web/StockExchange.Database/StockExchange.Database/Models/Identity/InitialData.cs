using System.Collections.Generic;

namespace StockExchange.Models
{
    public class InitialData
    {
        public static SiteOwnedStocks Create(int fP, int fPL, int pGB, int fPC, int fPA, int dL24, string site_Owned_Id)
        {
            var siteStocks = new List<OwnedStock>() {
                    new OwnedStock
                    {
                        Name = "FP",
                        Value = fP
                    },
                    new OwnedStock
                    {
                        Name = "FPL",
                        Value = fPL
                    },
                    new OwnedStock
                    {
                        Name = "PGB",
                        Value = pGB
                    },
                    new OwnedStock
                    {
                        Name = "FPC",
                        Value = fPC
                    },
                    new OwnedStock
                    {
                        Name = "FPA",
                        Value = fPA
                    },
                    new OwnedStock
                    {
                        Name = "DL24",
                        Value = dL24
                    },
            };
            var siteStocksDbObject = new SiteOwnedStocks
            {
                Id = site_Owned_Id,
                OwnedStocks = siteStocks
            };
            return siteStocksDbObject;
        }
    }
}
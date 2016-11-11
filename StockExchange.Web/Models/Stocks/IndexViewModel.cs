using System;
using System.Collections.Generic;

namespace StockExchange.Models
{
    // Main model used by the 'Index' view and partial views
    public class StocksIndexViewModel
    {
        public bool Success;
        public string Message;
        public decimal AccountBalance;
        public IList<OwnedStock> OwnedStocks;
        public IList<Item> StockPrices { get; set; }
        public DateTime PublicationDate;
        public IList<OwnedStock> SiteOwnedStocks;
        public IList<StocksTableData> StocksTableData;
    }
}
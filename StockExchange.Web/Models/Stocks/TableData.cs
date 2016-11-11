namespace StockExchange.Models
{
    // Used to build the buy/sell tables for stocks.
    public class StocksTableData
    {
        public string Name;
        public string Code;
        public int NumberOfStocks;
        public int NumberOfSiteStocks;
        public double Price;
        public double Value;
        public string StocksPriceId;
    }
}
using System;
using System.Collections.Generic;

namespace StockExchange.Models
{
    // Return format for the price list received from external provider
    public class Item
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Unit { get; set; }
        public double Price { get; set; }
    }
    public class StocksJsonData
    {
        public DateTime PublicationDate { get; set; }
        public IList<Item> Items { get; set; }
    }
}
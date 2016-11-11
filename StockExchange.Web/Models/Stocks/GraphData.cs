using System.Collections.Generic;

namespace StockExchange.Models
{
    // Format used for storing the price readings for the graph
    public class GraphData
    {
        public List<int> Times;
        public decimal[] Prices = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }
}
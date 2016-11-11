using System.ComponentModel.DataAnnotations;

namespace StockExchange.Models
{
    // Models a single stock owned by the user or the site
    public class OwnedStock
    {
        public int Id { get; set; }

        [Range(0, 1000000, ErrorMessage = "Invalid number of stocks: Requires an integer not higher than 100000.")]
        [Display(Name = "Number of stocks")]
        public int Value { get; set; }

        public string Name { get; set; }
    }
}
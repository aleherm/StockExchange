using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Models
{
    // Used to separate user owned stocks from the ones owned by the site
    public class SiteOwnedStocks
    {
        [Key]
        public string Id { get; set; }
        public virtual IList<OwnedStock> OwnedStocks { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockExchange.Models
{
    // Used by the 'EditWallet' partial view.
    public class EditWalletViewModel
    {
        public string Message;

        public int CompanyCount;
        public int NumberOfColumns;

        public IList<OwnedStock> OwnedStocks;
        public IList<OwnedStock> ModifiedOwnedStocks { get; set; }

        public decimal AccountBalance;
        [Display(Name = "Account Balance")]
        [Range(0, 1000000, ErrorMessageResourceName = "ValidationAccBalanceOOR", ErrorMessageResourceType = typeof(StockExResr))]
        public decimal ModifiedAccountBalance { get; set; }
    }
}
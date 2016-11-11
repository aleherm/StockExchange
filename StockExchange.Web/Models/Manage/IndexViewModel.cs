using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace StockExchange.Models
{
    // Main model used by the 'Index' view.
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public EditWalletViewModel EditWalletViewModel;
    }
}
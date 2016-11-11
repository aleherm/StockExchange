using System.ComponentModel.DataAnnotations;

namespace StockExchange.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessageResourceName = "ValidationEmailCharLimit", ErrorMessageResourceType = typeof(StockExResr))]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Currency (PLN)")]
        [Range(0, 1000000, ErrorMessageResourceName = "ValidationAccBalanceOOR", ErrorMessageResourceType = typeof(StockExResr))]
        public int AccountBalance { get; set; }

        [Display(Name = "FP")]
        [Range(0, 1000, ErrorMessageResourceName = "ValidationStocksLimit", ErrorMessageResourceType = typeof(StockExResr))]
        public int FP { get; set; }

        [Display(Name = "FPL")]
        [Range(0, 1000, ErrorMessageResourceName = "ValidationStocksLimit", ErrorMessageResourceType = typeof(StockExResr))]
        public int FPL { get; set; }

        [Display(Name = "PGB")]
        [Range(0, 1000, ErrorMessageResourceName = "ValidationStocksLimit", ErrorMessageResourceType = typeof(StockExResr))]
        public int PGB { get; set; }

        [Display(Name = "FPC")]
        [Range(0, 1000, ErrorMessageResourceName = "ValidationStocksLimit", ErrorMessageResourceType = typeof(StockExResr))]
        public int FPC { get; set; }

        [Display(Name = "FPA")]
        [Range(0, 1000, ErrorMessageResourceName = "ValidationStocksLimit", ErrorMessageResourceType = typeof(StockExResr))]
        public int FPA { get; set; }

        [Display(Name = "DL24 ")]
        [Range(0, 1000, ErrorMessageResourceName = "ValidationStocksLimit", ErrorMessageResourceType = typeof(StockExResr))]
        public int DL24 { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "ValidationPasswordCharLimit", ErrorMessageResourceType = typeof(StockExResr))]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessageResourceName = "ValidationPasswordCharLimit", ErrorMessageResourceType = typeof(StockExResr))]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessageResourceName = "ValidationPasswordMismatch", ErrorMessageResourceType = typeof(StockExResr))]
        //ErrorMessage = StockExResr.ValidationPasswordMismatch
        public string ConfirmPassword { get; set; }
    }
}
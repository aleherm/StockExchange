using System.ComponentModel.DataAnnotations;

namespace StockExchange.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessageResourceName = "ValidationEmailCharLimit", ErrorMessageResourceType = typeof(StockExResr))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [StringLength(100, ErrorMessageResourceName = "ValidationPasswordCharLimit", ErrorMessageResourceType = typeof(StockExResr))]
        [Compare("Password", ErrorMessageResourceName = "ValidationPasswordMismatch", ErrorMessageResourceType = typeof(StockExResr))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
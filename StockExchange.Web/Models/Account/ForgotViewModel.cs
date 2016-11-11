using System.ComponentModel.DataAnnotations;

namespace StockExchange.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
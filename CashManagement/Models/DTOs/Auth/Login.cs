using System.ComponentModel.DataAnnotations;

namespace CashManagement.Models.DTOs.Auth
{
    public class Login
    {
        [Required(ErrorMessage ="Email required")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

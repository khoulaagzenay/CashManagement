using CashManagement.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashManagement.Models.Entities.Authentication
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime DateExpire { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }

    }
}

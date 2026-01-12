using System.ComponentModel.DataAnnotations;

namespace CashManagement.Models.Entities
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string? BankName { get; set; }
        [MaxLength(34)]
        public string Iban { get; set; }
        [Required]
        public string Currency { get; set; } = "EUR";
        [Required(ErrorMessage = "Type required : CHECKING, SAVINGS, PSP")]
        public string Type { get; set; } 
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<DailyBalance> DailyBalances { get; set; } = new List<DailyBalance>();

    }
}

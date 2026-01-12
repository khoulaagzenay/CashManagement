using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashManagement.Models.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
        [Required]
        [MaxLength(3)]
        public string Direction { get; set; } 
        [Required]
        [Column(TypeName = "decimal(15,2)")]
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(150)]
        public string? Counterparty { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime BookingDate { get; set; }
        [Required]
        [MaxLength(30)]
        public string Status { get; set; }
        public bool IsRecurring { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}

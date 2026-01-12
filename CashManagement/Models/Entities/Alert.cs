using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashManagement.Models.Entities
{
    public class Alert
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = default!;
        [Column(TypeName = "decimal(15,2)")]
        public decimal Threshold { get; set; }
        public Guid? AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }
        [Required]
        [MaxLength(100)]
        public string Channel { get; set; } = "EMAIL";
        public bool IsEnabled { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

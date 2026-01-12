using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CashManagement.Models.Entities
{
    public class DailyBalance
    {
        [Key] 
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        [JsonIgnore]
        public Account Account { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(15,2)")]
        public decimal OpeningBalance { get; set; }
        [Column(TypeName = "decimal(15,2)")]
        public decimal Inflows { get; set; }
        [Column(TypeName = "decimal(15,2)")]
        public decimal Outflows { get; set; }
        [Column(TypeName = "decimal(15,2)")]
        public decimal ClosingBalance { get; set; }
    }
}

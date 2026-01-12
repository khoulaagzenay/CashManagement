using System.ComponentModel.DataAnnotations;

namespace CashManagement.Models.DTOs
{
    public class CreateTransactionDto
    {
        [Required(ErrorMessage = "AccountId required")]
        public Guid AccountId { get; set; }
        [Required(ErrorMessage = "Direction required : IN / OUT")]
        public string Direction { get; set; }
        [Required(ErrorMessage = "Amount required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }
        public string? Category { get; set; }
        public string? Counterparty { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }; 
        public bool IsRecurring { get; set; } = false;
    }
}

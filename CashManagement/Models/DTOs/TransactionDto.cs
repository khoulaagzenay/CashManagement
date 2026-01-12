namespace CashManagement.Models.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Direction { get; set; } // IN / OUT
        public decimal Amount { get; set; }
        public string? Category { get; set; }
        public string? Counterparty { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public bool IsRecurring { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

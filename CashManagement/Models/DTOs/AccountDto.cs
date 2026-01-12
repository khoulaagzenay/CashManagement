namespace CashManagement.Models.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? BankName { get; set; }
        public string Iban { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } 
        public decimal CurrentBalance { get; set; }
    }
}

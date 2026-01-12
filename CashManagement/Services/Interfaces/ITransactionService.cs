using CashManagement.Models.DTOs;

namespace CashManagement.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto?> GetByIdAsync(Guid id); 
        Task<IEnumerable<TransactionDto>> GetByAccountAsync(Guid accountId); 
        Task<TransactionDto> CreateAsync(CreateTransactionDto dto); 
        Task DeleteAsync(Guid id);
    }
}

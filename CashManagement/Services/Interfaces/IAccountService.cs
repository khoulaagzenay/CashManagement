using CashManagement.Models.DTOs;
using CashManagement.Models.Entities;

namespace CashManagement.Services.Interfaces
{
    public interface IAccountService 
    {
        Task<AccountDto?> GetByIdAsync(Guid id); 
        Task<IEnumerable<AccountDto>> GetAllAsync(); 
        Task<AccountDto> CreateAsync(AccountDto dto); 
        Task UpdateAsync(Guid id, AccountDto dto); 
        Task DeleteAsync(Guid id); 
        Task<decimal> GetBalanceAsync(Guid accountId); 
        Task<bool> AccountExistsAsync(Guid accountId);
    }
}

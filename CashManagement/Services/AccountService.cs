using CashManagement.Data.UnitOfWork;
using CashManagement.Models.DTOs;
using CashManagement.Models.Entities;
using CashManagement.Services.Interfaces;

namespace CashManagement.Services
{
    public class AccountService : IAccountService
    {
            private readonly IUnitOfWork _unitOfWork;

            public AccountService(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }
            private AccountDto ToDto(Account account, decimal balance)
            {
                return new AccountDto
                {
                    Id = account.Id,
                    Name = account.Name,
                    BankName = account.BankName,
                    Iban = account.Iban,
                    Currency = account.Currency,
                    Type = account.Type,
                    IsActive = account.IsActive,
                    CreatedAt = account.CreatedAt,
                    CurrentBalance = balance
                };
            }

            private Account ToEntity(AccountDto dto)
            {
                return new Account
                {
                    Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                    Name = dto.Name,
                    BankName = dto.BankName,
                    Iban = dto.Iban,
                    Currency = dto.Currency,
                    Type = dto.Type,
                    IsActive = dto.IsActive
                };
            }
            public async Task<AccountDto?> GetByIdAsync(Guid id)
            {
                var account = await _unitOfWork.Accounts.GetByIdAsync(id);
                if (account == null) return null;

                var balance = await GetBalanceAsync(id);
                return ToDto(account, balance);
            }

            public async Task<IEnumerable<AccountDto>> GetAllAsync()
            {
                var accounts = await _unitOfWork.Accounts.GetAllAsync();
                var result = new List<AccountDto>();

                foreach (var acc in accounts)
                {
                    var balance = await GetBalanceAsync(acc.Id);
                    result.Add(ToDto(acc, balance));
                }

                return result;
            }

            public async Task<AccountDto> CreateAsync(AccountDto dto)
            {
                var entity = ToEntity(dto);
            if (dto.Id != Guid.Empty)
            {
                var existing = await _unitOfWork.Accounts.GetByIdAsync(dto.Id);
                if (existing != null)
                    throw new InvalidOperationException("An account with the provided ID already exists. Remove the ID from the payload so the server generates a new ID..");
            }

            await _unitOfWork.Accounts.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            if (dto.CurrentBalance != 0m)
            {
                var amount = Math.Abs(dto.CurrentBalance);
                var direction = dto.CurrentBalance >= 0m ? "IN" : "OUT";

                var initialTransaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = entity.Id,
                    Direction = direction,
                    Amount = amount,
                    ExecutionDate = DateTime.UtcNow,
                    BookingDate = DateTime.UtcNow,
                    Status = "BOOKED",
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Transactions.AddAsync(initialTransaction);
                await _unitOfWork.SaveChangesAsync();
            }

            var balance = await GetBalanceAsync(entity.Id);
                return ToDto(entity, balance);
            }

            public async Task UpdateAsync(Guid id, AccountDto dto)
            {
                var existing = await _unitOfWork.Accounts.GetByIdAsync(id);
                if (existing == null)
                    throw new KeyNotFoundException("Account not found.");

                existing.Name = dto.Name;
                existing.BankName = dto.BankName;
                existing.Iban = dto.Iban;
                existing.Currency = dto.Currency;
                existing.Type = dto.Type;
                existing.IsActive = dto.IsActive;

                await _unitOfWork.Accounts.UpdateAsync(existing);
                await _unitOfWork.SaveChangesAsync();
            }

            public async Task DeleteAsync(Guid id)
            {
                var hasTransactions = (await _unitOfWork.Transactions
                    .FindAsync(t => t.AccountId == id))
                    .Any();

                if (hasTransactions)
                    throw new InvalidOperationException("It is impossible to delete an account with transactions.");

                await _unitOfWork.Accounts.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }

            public async Task<decimal> GetBalanceAsync(Guid accountId)
            {
                var transactions = await _unitOfWork.Transactions
                    .FindAsync(t => t.AccountId == accountId);

                return transactions.Sum(t => t.Amount);
            }
            public async Task<bool> AccountExistsAsync(Guid accountId)
            {
                return await _unitOfWork.Accounts.GetByIdAsync(accountId) != null;
            }
        
    }
}


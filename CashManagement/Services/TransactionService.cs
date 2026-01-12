using CashManagement.Data.UnitOfWork;
using CashManagement.Models.DTOs;
using CashManagement.Models.Entities;
using CashManagement.Services.Interfaces;
using System.Transactions;

namespace CashManagement.Services
{ 
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private TransactionDto ToDto(Models.Entities.Transaction t) 
        { 
            return new TransactionDto 
            { 
                Id = t.Id, 
                AccountId = t.AccountId, 
                Direction = t.Direction, 
                Amount = t.Amount, 
                Category = t.Category, 
                Counterparty = t.Counterparty, 
                ExecutionDate = t.ExecutionDate, 
                BookingDate = t.BookingDate, 
                Status = t.Status, 
                IsRecurring = t.IsRecurring, 
                CreatedAt = t.CreatedAt 
            }; 
        }
        private Models.Entities.Transaction ToEntity(CreateTransactionDto dto) 
        { 
            return new Models.Entities.Transaction 
        { 
            Id = Guid.NewGuid(), 
            AccountId = dto.AccountId, 
            Direction = dto.Direction, 
            Amount = dto.Amount, 
            Category = dto.Category, 
            Counterparty = dto.Counterparty, 
            ExecutionDate = dto.ExecutionDate, 
            BookingDate = dto.BookingDate, 
            Status = dto.Status, 
            IsRecurring = dto.IsRecurring 
        }; 
        }
        public async Task<TransactionDto?> GetByIdAsync(Guid id) 
        { 
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id); 
            return transaction == null ? null : ToDto(transaction); 
        } 
        public async Task<IEnumerable<TransactionDto>> GetByAccountAsync(Guid accountId) 
        { 
            var list = await _unitOfWork.Transactions.FindAsync(t => t.AccountId == accountId); 
            return list.Select(ToDto); 
        }
        public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
        { 
            var account = await _unitOfWork.Accounts.GetByIdAsync(dto.AccountId); 
            if (account == null) throw new KeyNotFoundException("Account not found"); 
            var entity = ToEntity(dto); 
            await _unitOfWork.Transactions.AddAsync(entity); 
            await _unitOfWork.SaveChangesAsync(); 
            return ToDto(entity);
        } 
        public async Task DeleteAsync(Guid id) 
        { 
            var existing = await _unitOfWork.Transactions.GetByIdAsync(id); 
            if (existing == null) 
                throw new KeyNotFoundException("Transaction not found."); 
            await _unitOfWork.Transactions.DeleteAsync(id); 
            await _unitOfWork.SaveChangesAsync(); 
        }
    }
}

  


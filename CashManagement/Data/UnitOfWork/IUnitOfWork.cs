using CashManagement.Data.Repositories;
using CashManagement.Models.Entities;
using CashManagement.Models.Entities.Authentication;

namespace CashManagement.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Account> Accounts { get; }
        IGenericRepository<Transaction> Transactions { get; }
        IGenericRepository<Alert> Alerts { get; }
        IGenericRepository<DailyBalance> DailyBalances { get; }
        IGenericRepository<RefreshToken> RefreshTokens { get; }


        Task<int> SaveChangesAsync();
    }
}

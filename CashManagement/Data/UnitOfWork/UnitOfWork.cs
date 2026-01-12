using CashManagement.Data.Repositories;
using CashManagement.Models.Entities;
using CashManagement.Models.Entities.Authentication;

namespace CashManagement.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Accounts = new GenericRepository<Account>(_context);
            Transactions = new GenericRepository<Transaction>(_context);
            Alerts = new GenericRepository<Alert>(_context);
            DailyBalances = new GenericRepository<DailyBalance>(_context);
            RefreshTokens = new GenericRepository<RefreshToken>(_context);
        }
        public IGenericRepository<Account> Accounts { get; }
        public IGenericRepository<Transaction> Transactions { get; }
        public IGenericRepository<Alert> Alerts { get; }
        public IGenericRepository<DailyBalance> DailyBalances { get; }
        public IGenericRepository<RefreshToken> RefreshTokens { get; }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

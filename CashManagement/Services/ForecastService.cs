using CashManagement.Data.UnitOfWork;
using CashManagement.Models.Entities;
using CashManagement.Services.Interfaces;

namespace CashManagement.Services
{
    public class ForecastService : IForecastService
    {
            private readonly IUnitOfWork _unitOfWork;

            public ForecastService(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DailyBalance?> GetDailyBalanceAsync(Guid accountId, DateTime date)
            {
                var result = await _unitOfWork.DailyBalances
                    .FindAsync(d => d.AccountId == accountId && d.Date.Date == date.Date);

                return result.FirstOrDefault();
            }

            public async Task<IEnumerable<DailyBalance>> GenerateDailyBalancesAsync(Guid accountId, DateTime startDate, DateTime endDate)
            {
                if (startDate > endDate)
                    throw new ArgumentException("Start date must be before end date.");

                var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
                if (account == null)
                    throw new KeyNotFoundException("Account not found.");

                var transactions = (await _unitOfWork.Transactions
                    .FindAsync(t => t.AccountId == accountId))
                    .OrderBy(t => t.BookingDate)
                    .ToList();
                Console.WriteLine("=== DEBUG TRANSACTIONS ===");
                Console.WriteLine($"AccountId  : {accountId}");
                Console.WriteLine($"Transactions : {transactions.Count}");
                foreach (var t in transactions)
                {
                    Console.WriteLine($"{t.Id} | Direction: {t.Direction} | Amount: {t.Amount} | BookingDate: {t.BookingDate:O}");
                }
                decimal previousClosingBalance = transactions
                    .Where(t => t.BookingDate.Date < startDate.Date)
                    .Sum(t => string.Equals(t.Direction, "out", StringComparison.OrdinalIgnoreCase) ? -t.Amount : t.Amount);

                var dailyBalances = new List<DailyBalance>();

                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    var inflows = transactions
                        .Where(t => t.BookingDate.Date == date &&
                                    string.Equals(t.Direction, "in", StringComparison.OrdinalIgnoreCase))
                        .Sum(t => t.Amount);

                    var outflows = transactions
                        .Where(t => t.BookingDate.Date == date &&
                                    string.Equals(t.Direction, "out", StringComparison.OrdinalIgnoreCase))
                        .Sum(t => t.Amount);

                    var daily = new DailyBalance
                    {
                        Id = Guid.NewGuid(),
                        AccountId = accountId,
                        Date = date,
                        OpeningBalance = previousClosingBalance,
                        Inflows = inflows,
                        Outflows = outflows,
                        ClosingBalance = previousClosingBalance + inflows - outflows
                    };

                    dailyBalances.Add(daily);

                    previousClosingBalance = daily.ClosingBalance;
                }

                var old = await _unitOfWork.DailyBalances.FindAsync(d => d.AccountId == accountId);
                foreach (var d in old)
                    await _unitOfWork.DailyBalances.DeleteAsync(d.Id);

                foreach (var d in dailyBalances)
                    await _unitOfWork.DailyBalances.AddAsync(d);

                await _unitOfWork.SaveChangesAsync();

                return dailyBalances;
            }   
    }
}




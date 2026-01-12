using CashManagement.Models.Entities;

namespace CashManagement.Services.Interfaces
{
    public interface IForecastService
    {
        Task<DailyBalance?> GetDailyBalanceAsync(Guid accountId, DateTime date); 
        Task<IEnumerable<DailyBalance>> GenerateDailyBalancesAsync(Guid accountId, DateTime startDate, DateTime endDate);
    }

}


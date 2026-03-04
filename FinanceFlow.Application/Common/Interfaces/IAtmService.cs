using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.DTOs;

namespace FinanceFlow.Application.Common.Interfaces
{
    public interface IAtmService
    {
        Task<bool> BankFeedAsync(int atmId, int amount, string description);
        Task<decimal> GetUserBalanceAsync(string accountId);
        Task<int> GetAtmBalanceAsync(int atmId);
        Task<IReadOnlyList<AtmDto>> GetAllAtmsAsync();
    }
}


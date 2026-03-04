using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Common.Interfaces
{
    public interface IDepositeService
    {
        Task<bool> Deposite(int amount, string AccountId,int AtmId, string disc);
        Task<Dictionary<int, int>> Calc(int amount);

    }
}

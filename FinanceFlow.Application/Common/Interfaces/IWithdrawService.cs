using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceFlow.Application.Common.Interfaces
{
    public interface IWithdrawService
    {
        public Task<bool> Withdraw(int amount, string AccountId, int AtmId, string disc);
        public Task<bool> CheckValidAmount(int amount, string AccountId,int AtmId);
    }
}

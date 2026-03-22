using System.Threading.Tasks;

namespace FinanceFlow.Application.Common.Interfaces
{
    public interface ITransferService
    {
        Task<bool> TransferAsync(string fromAccountId, string toAccountId, int amount ,string description);
    }
}


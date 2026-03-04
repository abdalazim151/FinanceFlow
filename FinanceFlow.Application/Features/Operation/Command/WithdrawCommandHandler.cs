using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public record WithdrawCommand(
        int Amount,
        string Description,
        string AccountId,
        int AtmId
    ) : IRequest<bool>;

    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, bool>
    {
        private readonly IWithdrawService withdrawService;

        public WithdrawCommandHandler(IWithdrawService withdrawService)
        {
            this.withdrawService = withdrawService;
        }

        public async Task<bool> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var isValid = await withdrawService.CheckValidAmount(
                request.Amount,
                request.AccountId,
                request.AtmId
            );

            if (!isValid)
            {
                return false;
            }

            return await withdrawService.Withdraw(
                request.Amount,
                request.AccountId,
                request.AtmId,
                request.Description
            );
        }
    }
}

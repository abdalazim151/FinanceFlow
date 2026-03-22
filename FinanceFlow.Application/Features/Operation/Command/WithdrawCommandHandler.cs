using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public record WithdrawCommand(
        int Amount,
        string Description,
        string? AccountId,
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
/*
  eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjExOTE4NWNjLTczY2MtNGRjYS04NDBjLTBmZTM3MmZkZDhmZiIsImV4cCI6MTc3MjY0NTkwMSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDoxNzE0LyIsImF1ZCI6IkFiZG8ifQ.OKbVoWH7CL9DMDFz_zPbTOa-eLrhORcs6-VuUYc37rU


*/
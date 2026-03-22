using System.Threading;
using System.Threading.Tasks;
using FinanceFlow.Application.Common.Interfaces;
using MediatR;

namespace FinanceFlow.Application.Features.Operation.Command
{
    public record DepositeCommand(
        int Amount,
        string Description,
        string? AccountId,
        int AtmId
    ) : IRequest<bool>;

    public class DepositeCommandHandler : IRequestHandler<DepositeCommand, bool>
    {
        private readonly IDepositeService depositeService;

        public DepositeCommandHandler(IDepositeService depositeService)
        {
            this.depositeService = depositeService;
        }

        public async Task<bool> Handle(DepositeCommand request, CancellationToken cancellationToken)
        {
            return await depositeService.Deposite(
                request.Amount,
                request.AccountId,
                request.AtmId,
                request.Description
            );
        }
    }
}
